﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour {
    public GameObject target;
    private Rigidbody shipRigidbody;
    public float moveStrength = 200.0f;
    public float rotationStrength = 0.1f;
    public float stoppingDistance = 40.0f;

    private Quaternion lookRotation;
    private Vector3 direction;
    private float distance;
    private Transform shipTransform;

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
        float angle = Vector3.Angle(-transform.forward, transform.position - target.transform.position);

        distance = Vector3.Distance(target.transform.position, transform.position);
        direction = (target.transform.position - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            if(angle < 20)
            {
                //we are too far away
                float targetVelocity = (distance - stoppingDistance) / 60;
                if (transform.InverseTransformDirection(shipRigidbody.velocity).z < targetVelocity)
                {
                    shipRigidbody.AddForce(direction * (moveStrength - angle));
                }
                else
                {
                    shipRigidbody.AddForce(-direction * (moveStrength - angle));
                }
            }

            RotateTowardsTarget();
        }
        else
        {
            //we are withing firing distance. Rotate ship sideways.
            //RotateSidewaysFromTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        lookRotation = Quaternion.LookRotation(direction);
        shipRigidbody.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationStrength);
    }

    private void RotateSidewaysFromTarget()
    {
        lookRotation = Quaternion.LookRotation(direction, transform.forward);
        shipRigidbody.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationStrength);
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
}