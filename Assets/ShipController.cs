using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private FlightController flightController;
    private ShipAI shipAi;
    private WeaponController weaponController;
    private ShipHealth shipHealth;
    private Rigidbody shipRigidbody;
    private AutoTurretManager autoTurretManager;

    private bool playerControlled = false;
    private int count = 0;

    // Use this for initialization
    void Start()
    {
        flightController = GetComponentInChildren<FlightController>();
        shipAi = GetComponent<ShipAI>();
        shipRigidbody = GetComponent<Rigidbody>();
        shipHealth = GetComponentInChildren<ShipHealth>();
        weaponController = GetComponentInChildren<WeaponController>();
        autoTurretManager = GetComponentInChildren<AutoTurretManager>();
    }

    void FixedUpdate()
    {
        if (playerControlled)
        {
            flightController.Fly();
        }
        else
        {
            shipAi.Fly();
        }
    }

    void Update()
    {
        count++;
        if (count < 30) { return; }
        count = 0;

        if (!playerControlled && shipAi.AIisActive)
        {
            flightController.ActivateThrustersAI(shipAi.isMovingForward, shipAi.isMovingBackward);
        }
        else
        {
            flightController.ActivateThrustersPlayer();
        }
    }

    public void SetPlayerControlled(bool pControlled)
    {
        playerControlled = pControlled;

        weaponController.SetPlayerControlled(pControlled);
        shipHealth.SetPlayerControlled(pControlled);
    }

    public void ToggleRotationalStabilisers()
    {
        flightController.ToggleRotationalStabilisers();
    }

    public void ToggleMovementStabilisers()
    {
        flightController.ToggleMovementStabilisers();
    }

    public bool AreMovementStabilisersActive()
    {
        return flightController.AreMovementStabilisersActive();
    }

    public bool AreRotationalStabilisersActive()
    {
        return flightController.AreRotationalStabilisersActive();
    }

    public Rigidbody GetRigidbody()
    {
        return shipRigidbody;
    }

    public void SetAiTarget(GameObject target)
    {
        if (shipAi == null)
        {
            shipAi = GetComponent<ShipAI>();
        }
        shipAi.target = target;
    }

    public void RemoveEnemyTarget(GameObject ship)
    {
        autoTurretManager.RemoveEnemyShip(ship);
    }

    public void SetEnemyShips(List<GameObject> ship)
    {
        if (autoTurretManager == null)
        {
            autoTurretManager = GetComponentInChildren<AutoTurretManager>();
        }
        autoTurretManager.SetEnemyShips(ship);
    }

    public void DestroyShip()
    {
        flightController.DisableAllThrusters();

        foreach (MonoBehaviour m in GetComponentsInChildren<MonoBehaviour>())
        {
            m.enabled = false;
        }
    }
}
