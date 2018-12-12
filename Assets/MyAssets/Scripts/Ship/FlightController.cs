using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public float forwardThrust = 10.0f;
    public float horizontalThrust = 10.0f;
    public float verticalThrust = 10.0f;

    public float yawSpeed = 0.03f;
    public float rollSpeed = 0.03f;
    public float pitchSpeed = 0.03f;

    private Rigidbody shipRigidbody;

    private float moveForward;
    private float moveHorizontal;
    private float moveVertical;

    private float yaw;
    private float pitch;
    private float roll;

    private bool rotationalStabiliersActive = false;
    private bool movementStabiliersActive = false;

    private ThrusterParticlesController thrusterParticlesController;

    void Awake()
    {
        shipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        thrusterParticlesController = gameObject.GetComponent<ThrusterParticlesController>();
    }

    public void Fly()
    {
        moveForward = Input.GetAxis("Move Forward") - Input.GetAxis("Move Backward");
        moveHorizontal = Input.GetAxis("Move Right") - Input.GetAxis("Move Left");
        moveVertical = Input.GetAxis("Move Up") - Input.GetAxis("Move Down");
        pitch = Input.GetAxis("Pitch Down") - Input.GetAxis("Pitch Up");
        yaw = Input.GetAxis("Yaw Right") - Input.GetAxis("Yaw Left");
        roll = Input.GetAxis("Roll Left") - Input.GetAxis("Roll Right");

        ThrusterMovement();
        ThrusterRotation();

        if (rotationalStabiliersActive)
        {
            StabiliseRotation();
        }

        if (movementStabiliersActive)
        {
            StabiliseMovement();
        }
    }

    public void ActivateThrustersPlayer()
    {
        thrusterParticlesController.ActivateThrustersPlayer(shipRigidbody, movementStabiliersActive, rotationalStabiliersActive);
    }

    public void ActivateThrustersAI(bool aiMovingForward, bool aiMovingBackward)
    {
        thrusterParticlesController.ActivateThrustersAI(shipRigidbody, aiMovingForward, aiMovingBackward);
    }

    public void ToggleRotationalStabilisers()
    {
        rotationalStabiliersActive = !rotationalStabiliersActive;
    }

    public void ToggleMovementStabilisers()
    {
        movementStabiliersActive = !movementStabiliersActive;
    }

    public bool AreMovementStabilisersActive()
    {
        return movementStabiliersActive;
    }

    public bool AreRotationalStabilisersActive()
    {
        return rotationalStabiliersActive;
    }

    private void StabiliseRotation()
    {
        if (pitch == 0)
        {
            shipRigidbody.angularVelocity = new Vector3(shipRigidbody.angularVelocity.x * 0.95f, shipRigidbody.angularVelocity.y, shipRigidbody.angularVelocity.z);
        }
        if (yaw == 0)
        {
            shipRigidbody.angularVelocity = new Vector3(shipRigidbody.angularVelocity.x, shipRigidbody.angularVelocity.y * 0.95f, shipRigidbody.angularVelocity.z);
        }
        if (roll == 0)
        {
            shipRigidbody.angularVelocity = new Vector3(shipRigidbody.angularVelocity.x, shipRigidbody.angularVelocity.y, shipRigidbody.angularVelocity.z * 0.95f);
        }
    }

    private void StabiliseMovement()
    {
        if (moveHorizontal == 0)
        {
            float localXVelocity = base.transform.InverseTransformDirection(shipRigidbody.velocity).x;
            DetermineStabilisingSpeed(shipRigidbody, localXVelocity, transform.right, horizontalThrust);
        }

        if (moveVertical == 0)
        {
            float localYVelocity = base.transform.InverseTransformDirection(shipRigidbody.velocity).y;
            DetermineStabilisingSpeed(shipRigidbody, localYVelocity, transform.up, verticalThrust);
        }

        if (moveForward == 0)
        {
            float localZVelocity = base.transform.InverseTransformDirection(shipRigidbody.velocity).z;
            DetermineStabilisingSpeed(shipRigidbody, localZVelocity, transform.forward, forwardThrust);
        }
    }

    public static void DetermineStabilisingSpeed(Rigidbody shipRigidbody, float currentVelocity, Vector3 direction, float thrust)
    {
        if (currentVelocity < 0.1f && currentVelocity > -0.1f)
        {
            //if ship velocity is low, reduce velocity by 90%. This allows the ship to come to a complete stop smoothly.
            SlowShipUsingStabilisers(shipRigidbody, currentVelocity, direction, thrust * 0.1f);
        }
        else
        {
            SlowShipUsingStabilisers(shipRigidbody, currentVelocity, direction, thrust);
        }
    }

    public static void SlowShipUsingStabilisers(Rigidbody shipRigidbody, float localVelocity, Vector3 direction, float thrustSpeed)
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

    public void DisableAllThrusters()
    {
        thrusterParticlesController.DisableAllThrusters();
    }

    private void ThrusterMovement()
    {
        shipRigidbody.AddForce(transform.forward * forwardThrust * moveForward);
        shipRigidbody.AddForce(transform.right * horizontalThrust * moveHorizontal);
        shipRigidbody.AddForce(transform.up * verticalThrust * moveVertical);
    }

    private void ThrusterRotation()
    {
        // Rotate about Y principal axis
        //Vector3 desiredAngularVelInY = Vector3.up * yawSpeed * yaw;
        //Vector3 torque = ShipRigidbody.inertiaTensorRotation * Vector3.Scale(ShipRigidbody.inertiaTensor, desiredAngularVelInY);
        //ShipRigidbody.AddRelativeTorque(torque, ForceMode.Impulse);

        shipRigidbody.AddTorque(transform.right * pitchSpeed * pitch);
        shipRigidbody.AddTorque(transform.up * yawSpeed * yaw);
        shipRigidbody.AddTorque(transform.forward * rollSpeed * roll);
    }
}
