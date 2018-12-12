using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour {
    public GameObject target;
    private Rigidbody shipRigidbody;
    public float moveStrength = 200.0f;
    public float rotationStrength = 0.1f;
    public float stoppingDistance = 40.0f;
    public bool AIisActive = true;

    private Quaternion lookRotation;
    private Vector3 targetDirection;
    private float distanceToTarget;
    private Transform shipTransform;
    private float angleToTarget;
    private float targetVelocity;
    private CannonManager cannonManager;

    public bool isMovingForward { set; get; }
    public bool isMovingBackward { set; get; }

    void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody>();
        cannonManager = GetComponentInChildren<CannonManager>();
    }

    public void Fly()
    {
        if (!AIisActive)
        {
            return;
        }
        if(target != null)
        {
            MoveTowardsTarget();
            cannonManager.SetCannonTarget(target);
            cannonManager.Fire();
        }
    }

    private void MoveTowardsTarget()
    {
        angleToTarget = Vector3.Angle(-transform.forward, transform.position - target.transform.position);
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceToTarget > stoppingDistance)
        {
            MoveToTarget();
        }
        else
        {
            AttackTarget();
        }

        StabiliseMovement();
        StabiliseRotation();
    }

    private void MoveToTarget()
    {
        targetDirection = (target.transform.position - transform.position).normalized;

        if (angleToTarget < 20)
        {
            targetVelocity = (distanceToTarget - stoppingDistance+10) / 40;
            if (transform.InverseTransformDirection(shipRigidbody.velocity).z < targetVelocity)
            {
                //accelerate
                shipRigidbody.AddForce(targetDirection * (moveStrength - angleToTarget));
                isMovingForward = true;
                isMovingBackward = false;
            }
            else
            {
                //decelerate
                shipRigidbody.AddForce(-targetDirection * (moveStrength - angleToTarget));
                isMovingForward = false;
                isMovingBackward = true;
            }
        }
        else
        {
            isMovingForward = false;
            isMovingBackward = false;
        }

        RotateShip(targetDirection);
    }

    private void AttackTarget()
    {
        //we are withing firing distance. Rotate ship sideways.
        RotateSidewaysFromTarget();
    }
   
    private void RotateSidewaysFromTarget()
    {        
        float angle = Vector3.Angle(-transform.forward, transform.position - target.transform.position);
        float angle2 = Vector3.Angle(transform.forward, transform.position - target.transform.position);
        
        if (angle < 90.5 && angle > 89.5)
        {
            return;
        }

        if (angle < angle2)
        {
            targetDirection = (target.transform.position - transform.InverseTransformDirection(transform.right)).normalized;
        }
        else
        {
            targetDirection = (target.transform.position - transform.InverseTransformDirection(-transform.right)).normalized;
        }

        RotateShip(targetDirection);
    }

    private void RotateShip(Vector3 target)
    {
        var x = Vector3.Cross(transform.forward.normalized, target.normalized);
        float theta = Mathf.Asin(x.magnitude);
        var w = x.normalized * theta / Time.fixedDeltaTime;
        var q = transform.rotation * shipRigidbody.inertiaTensorRotation;
        var t = q * Vector3.Scale(shipRigidbody.inertiaTensor, Quaternion.Inverse(q) * w);
        shipRigidbody.AddTorque((t - shipRigidbody.angularVelocity) * rotationStrength);
    }

    private void StabiliseMovement()
    {
        float localXVelocity = transform.InverseTransformDirection(shipRigidbody.velocity).x;
        FlightController.DetermineStabilisingSpeed(shipRigidbody, localXVelocity, transform.right, moveStrength * 2f);

        float localYVelocity = transform.InverseTransformDirection(shipRigidbody.velocity).y;
        FlightController.DetermineStabilisingSpeed(shipRigidbody, localYVelocity, transform.up, moveStrength * 2f);        
    }

    private void StabiliseRotation()
    {
        shipRigidbody.angularVelocity *= 0.99f;
    }
}
