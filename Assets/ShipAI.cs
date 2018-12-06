using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour {
    public GameObject target;
    private Rigidbody shipRigidbody;
    public float moveStrength = 200.0f;
    public float rotationStrength = 0.1f;
    public float stoppingDistance = 40.0f;

    private Quaternion lookRotation;
    private Vector3 targetDirection;
    private float distanceToTarget;
    private Transform shipTransform;
    private float angleToTarget;
    private float targetVelocity;

    void Start()
    {
        shipRigidbody = GetComponent<Rigidbody>();
    }

    public void Fly()
    {
        if(target != null)
        {
            MoveTowardsTarget();
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
            }
            else
            {
                //decelerate
                shipRigidbody.AddForce(-targetDirection * (moveStrength - angleToTarget));
            }
        }

        RotateTowardsTarget();
    }

    private void AttackTarget()
    {
        //we are withing firing distance. Rotate ship sideways.
        RotateSidewaysFromTarget();
    }

    private void RotateTowardsTarget()
    {
        lookRotation = Quaternion.LookRotation(targetDirection);
        shipRigidbody.rotation = Quaternion.RotateTowards(shipRigidbody.rotation, lookRotation, rotationStrength);
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

        lookRotation = Quaternion.LookRotation(targetDirection);
        shipRigidbody.rotation = Quaternion.RotateTowards(shipRigidbody.rotation, lookRotation, rotationStrength);
    }

    private void StabiliseMovement()
    {
        float localXVelocity = transform.InverseTransformDirection(shipRigidbody.velocity).x;
        DetermineStabilisingSpeed(localXVelocity, transform.right, moveStrength * 2f);

        float localYVelocity = transform.InverseTransformDirection(shipRigidbody.velocity).y;
        DetermineStabilisingSpeed(localYVelocity, transform.up, moveStrength * 2f);        
    }

    private void DetermineStabilisingSpeed(float currentVelocity, Vector3 direction, float thrust)
    {
        if (currentVelocity < 0.1f && currentVelocity > -0.1f)
        {
            //if ship velocity is low, reduce velocity by 90%. This allows the ship to come to a complete stop smoothly.
            SlowShipUsingStabilisers(currentVelocity, direction, thrust * 0.1f);
        }
        else
        {
            SlowShipUsingStabilisers(currentVelocity, direction, thrust);
        }
    }

    private void SlowShipUsingStabilisers(float localVelocity, Vector3 direction, float thrustSpeed)
    {
        if (localVelocity > 0.005)
        {
            shipRigidbody.AddForce(-direction * thrustSpeed);
        }
        else if (localVelocity < -0.005)
        {
            shipRigidbody.AddForce(direction * thrustSpeed);
        }
    }

    private void StabiliseRotation()
    {
        shipRigidbody.angularVelocity *= 0.99f;
    }
}
