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

    private bool PlayerControlled = false;

    private Rigidbody ShipRigidbody;

    private float moveForward;
    private float moveHorizontal;
    private float moveVertical;

    private float yaw;
    private float pitch;
    private float roll;

    private bool rotationalStabiliersActive = false;
    private bool movementStabiliersActive = false;

    private ThrusterParticlesController thrusterParticlesController;

    private ShipAI shipAI;

    void Start()
    {
        ShipRigidbody = gameObject.GetComponent<Rigidbody>();

        thrusterParticlesController = gameObject.GetComponent<ThrusterParticlesController>();
        shipAI = gameObject.GetComponent<ShipAI>();
    }

    void FixedUpdate()
    {
        if (PlayerControlled)
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

            thrusterParticlesController.ActivateThrusters(ShipRigidbody, movementStabiliersActive, rotationalStabiliersActive);
        }
        else
        {
            shipAI.Fly();
            thrusterParticlesController.ActivateThrusters(ShipRigidbody, true, true);
        }
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

    public void SetPlayerControlled(bool IsPlayerControlled)
    {
        PlayerControlled = IsPlayerControlled;

        if (thrusterParticlesController == null)
        {
            thrusterParticlesController = gameObject.GetComponent<ThrusterParticlesController>();
        }
        thrusterParticlesController.SetPlayerControlled(IsPlayerControlled);
    }

    private void StabiliseRotation()
    {
        if (pitch == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x * 0.95f, ShipRigidbody.angularVelocity.y, ShipRigidbody.angularVelocity.z);
        }
        if (yaw == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x, ShipRigidbody.angularVelocity.y * 0.95f, ShipRigidbody.angularVelocity.z);
        }
        if (roll == 0)
        {
            ShipRigidbody.angularVelocity = new Vector3(ShipRigidbody.angularVelocity.x, ShipRigidbody.angularVelocity.y, ShipRigidbody.angularVelocity.z * 0.95f);
        }
    }

    private void StabiliseMovement()
    {
        if (moveHorizontal == 0)
        {
            float localXVelocity = base.transform.InverseTransformDirection(ShipRigidbody.velocity).x;
            DetermineStabilisingSpeed(localXVelocity, transform.right, horizontalThrust);
        }

        if (moveVertical == 0)
        {
            float localYVelocity = base.transform.InverseTransformDirection(ShipRigidbody.velocity).y;
            DetermineStabilisingSpeed(localYVelocity, transform.up, verticalThrust);
        }

        if (moveForward == 0)
        {
            float localZVelocity = base.transform.InverseTransformDirection(ShipRigidbody.velocity).z;
            DetermineStabilisingSpeed(localZVelocity, transform.forward, forwardThrust);
        }
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
            ShipRigidbody.AddForce(-direction * thrustSpeed);
        }
        else if (localVelocity < -0.005)
        {
            ShipRigidbody.AddForce(direction * thrustSpeed);
        }
    }

    private void ThrusterMovement()
    {
        ShipRigidbody.AddForce(transform.forward * forwardThrust * moveForward);
        ShipRigidbody.AddForce(transform.right * horizontalThrust * moveHorizontal);
        ShipRigidbody.AddForce(transform.up * verticalThrust * moveVertical);
    }

    private void ThrusterRotation()
    {
        // Rotate about Y principal axis
        //Vector3 desiredAngularVelInY = Vector3.up * yawSpeed * yaw;
        //Vector3 torque = ShipRigidbody.inertiaTensorRotation * Vector3.Scale(ShipRigidbody.inertiaTensor, desiredAngularVelInY);
        //ShipRigidbody.AddRelativeTorque(torque, ForceMode.Impulse);

        ShipRigidbody.AddTorque(transform.right * pitchSpeed * pitch);
        ShipRigidbody.AddTorque(transform.up * yawSpeed * yaw);
        ShipRigidbody.AddTorque(transform.forward * rollSpeed * roll);
    }    
}
