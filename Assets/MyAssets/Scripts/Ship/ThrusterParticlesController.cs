using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterParticlesController : MonoBehaviour {

    public GameObject MoveForwardThrusters;
    public GameObject MoveBackwardThrusters;

    public GameObject MoveUpThrusters;
    public GameObject MoveDownThrusters;

    public GameObject MoveLeftThrusters;
    public GameObject MoveRightThrusters;

    public GameObject PitchUpThrusters;
    public GameObject PitchDownThrusters;

    public GameObject YawLeftThrusters;
    public GameObject YawRightThrusters;

    public GameObject RollLeftThrusters;
    public GameObject RollRightThrusters;

    private ParticleSystem[] MoveForwardThrustersParticles;
    private ParticleSystem[] MoveBackwardThrustersParticles;

    private ParticleSystem[] MoveUpThrustersParticles;
    private ParticleSystem[] MoveDownThrustersParticles;

    private ParticleSystem[] MoveLeftThrustersParticles;
    private ParticleSystem[] MoveRightThrustersParticles;

    private ParticleSystem[] PitchUpThrustersParticles;
    private ParticleSystem[] PitchDownThrustersParticles;

    private ParticleSystem[] YawLeftThrustersParticles;
    private ParticleSystem[] YawRightThrustersParticles;

    private ParticleSystem[] RollLeftThrustersParticles;
    private ParticleSystem[] RollRightThrustersParticles;

    void Start () {
        MoveForwardThrustersParticles = MoveForwardThrusters.GetComponentsInChildren<ParticleSystem>();
        MoveBackwardThrustersParticles = MoveBackwardThrusters.GetComponentsInChildren<ParticleSystem>();
        MoveUpThrustersParticles = MoveUpThrusters.GetComponentsInChildren<ParticleSystem>();
        MoveDownThrustersParticles = MoveDownThrusters.GetComponentsInChildren<ParticleSystem>();
        MoveLeftThrustersParticles = MoveLeftThrusters.GetComponentsInChildren<ParticleSystem>();
        MoveRightThrustersParticles = MoveRightThrusters.GetComponentsInChildren<ParticleSystem>();

        PitchUpThrustersParticles = PitchUpThrusters.GetComponentsInChildren<ParticleSystem>();
        PitchDownThrustersParticles = PitchDownThrusters.GetComponentsInChildren<ParticleSystem>();
        YawLeftThrustersParticles = YawLeftThrusters.GetComponentsInChildren<ParticleSystem>();
        YawRightThrustersParticles = YawRightThrusters.GetComponentsInChildren<ParticleSystem>();
        RollLeftThrustersParticles = RollLeftThrusters.GetComponentsInChildren<ParticleSystem>();
        RollRightThrustersParticles = RollRightThrusters.GetComponentsInChildren<ParticleSystem>();
    }

    public void ActivateThrustersPlayer(Rigidbody shipRigidbody, bool movementStabiliersActive, bool rotationalStabiliersActive)
    {
        DetermineThrusterActivation(Input.GetButton("Move Forward"), Input.GetButton("Move Backward"), MoveForwardThrustersParticles, MoveBackwardThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).z, movementStabiliersActive);
        DetermineThrusterActivation(Input.GetButton("Move Right"), Input.GetButton("Move Left"), MoveRightThrustersParticles, MoveLeftThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).x, movementStabiliersActive);
        DetermineThrusterActivation(Input.GetButton("Move Up"), Input.GetButton("Move Down"), MoveUpThrustersParticles, MoveDownThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).y, movementStabiliersActive);

        DetermineThrusterActivation(Input.GetButton("Pitch Up"), Input.GetButton("Pitch Down"), PitchUpThrustersParticles, PitchDownThrustersParticles, shipRigidbody.angularVelocity.x, rotationalStabiliersActive);
        DetermineThrusterActivation(Input.GetButton("Yaw Right"), Input.GetButton("Yaw Left"), YawRightThrustersParticles, YawLeftThrustersParticles, shipRigidbody.angularVelocity.y, rotationalStabiliersActive);
        DetermineThrusterActivation(Input.GetButton("Roll Right"), Input.GetButton("Roll Left"), RollRightThrustersParticles, RollLeftThrustersParticles, shipRigidbody.angularVelocity.z, rotationalStabiliersActive);
    }

    public void ActivateThrustersAI(Rigidbody shipRigidbody, bool aiMovingForward, bool aiMovingBackward)
    {
        DetermineThrusterActivation(aiMovingForward, aiMovingBackward, MoveForwardThrustersParticles, MoveBackwardThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).z, true);
        ActivateStabiliserThrusters(MoveRightThrustersParticles, MoveLeftThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).x);
        ActivateStabiliserThrusters(MoveUpThrustersParticles, MoveDownThrustersParticles, transform.InverseTransformDirection(shipRigidbody.velocity).y);

        ActivateStabiliserThrusters(PitchUpThrustersParticles, PitchDownThrustersParticles, shipRigidbody.angularVelocity.x);
        ActivateStabiliserThrusters(YawRightThrustersParticles, YawLeftThrustersParticles, shipRigidbody.angularVelocity.y);
        ActivateStabiliserThrusters(RollRightThrustersParticles, RollLeftThrustersParticles, shipRigidbody.angularVelocity.z);
    }

    private void DetermineThrusterActivation(bool positiveActive, bool negativeActive, ParticleSystem[] postive, ParticleSystem[] negative, float velocity, bool stabiliserActive)
    {
        if (positiveActive && negativeActive)
        {
            ToggleThrusterParticleSystems(postive, true);
            ToggleThrusterParticleSystems(negative, true);
        }
        else if (positiveActive)
        {
            ToggleThrusterParticleSystems(postive, true);
            ToggleThrusterParticleSystems(negative, false);
        }
        else if (negativeActive)
        {
            ToggleThrusterParticleSystems(postive, false);
            ToggleThrusterParticleSystems(negative, true);
        }
        else if (stabiliserActive)
        {
            //let stabilisers determine whihc thrusters are activated.
            ActivateStabiliserThrusters(postive, negative, velocity);
        }
        else
        {
            //Button not pressed and stabilisers deactivated. Turn off thrusters.
            ToggleThrusterParticleSystems(postive, false);
            ToggleThrusterParticleSystems(negative, false);
        }
    }

    private void ActivateStabiliserThrusters(ParticleSystem[] positive, ParticleSystem[] negative, float speed)
    {
        if (speed > 0.002)
        {
            ToggleThrusterParticleSystems(negative, true);
            ToggleThrusterParticleSystems(positive, false);
        }
        else if (speed < -0.002)
        {
            ToggleThrusterParticleSystems(positive, true);
            ToggleThrusterParticleSystems(negative, false);
        }
        else
        {
            ToggleThrusterParticleSystems(positive, false);
            ToggleThrusterParticleSystems(negative, false);
        }
    }

    private void ToggleThrusterParticleSystems(ParticleSystem[] ParticleSystems, bool enable)
    {
        foreach (ParticleSystem ps in ParticleSystems)
        {
            if (enable)
            {
                if (!ps.isPlaying) { ps.Play(); }
            }
            else
            {
                if (ps.isPlaying) { ps.Stop(); }
            }
        }
    }

    public void DisableAllThrusters()
    {
        ToggleThrusterParticleSystems(MoveForwardThrustersParticles, false);
        ToggleThrusterParticleSystems(MoveBackwardThrustersParticles, false);
        ToggleThrusterParticleSystems(MoveUpThrustersParticles, false);
        ToggleThrusterParticleSystems(MoveDownThrustersParticles, false);
        ToggleThrusterParticleSystems(MoveLeftThrustersParticles, false);
        ToggleThrusterParticleSystems(MoveRightThrustersParticles, false);
        ToggleThrusterParticleSystems(PitchUpThrustersParticles, false);
        ToggleThrusterParticleSystems(PitchDownThrustersParticles, false);
        ToggleThrusterParticleSystems(YawLeftThrustersParticles, false);
        ToggleThrusterParticleSystems(YawRightThrustersParticles, false);
        ToggleThrusterParticleSystems(RollLeftThrustersParticles, false);
        ToggleThrusterParticleSystems(RollRightThrustersParticles, false);
    }
}
