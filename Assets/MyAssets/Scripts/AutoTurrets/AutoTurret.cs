using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IAutoTurretWeapon
{
    void Fire(Collider target);
    void StopFiring();
    void Destroyed();
}

public class AutoTurret : MonoBehaviour
{
    public float FireRate = 2.0f;

    [SerializeField] private Transform turretTower;
    [SerializeField] private Transform turretGun;

    private IAutoTurretWeapon mainWeapon;
    private HashSet<Collider> enemyShips;
    private Collider currentTarget;
    private RaycastHit hit;
    private float updateCooldown;
    private int factionId;
    private int validTargetCounter;
    private int targetSearchCounter;
    private int layerMask;

    private void Awake()
    {
        layerMask = ~(1 << 2);
        enemyShips = new HashSet<Collider>();
        FactionController fc = gameObject.GetComponentInParent<FactionController>();
        if (fc == null)
        {
            factionId = -1;
        }
        else
        {
            factionId = fc.factionID;
        }

        updateCooldown = FireRate;

        mainWeapon = GetComponent<IAutoTurretWeapon>();
    }

    // Update is called once per frame
    private void Update()
    {
        updateCooldown -= Time.deltaTime;

        LookAtTarget();
        validTargetCounter++;
        targetSearchCounter++;

        //if gun cant shoot don't bother continuing.
        if (updateCooldown > 0)
        {
            return;
        }

        updateCooldown = FireRate;

        if (CheckTargetIsValid())
        {
            mainWeapon.Fire(currentTarget);
        }
        else
        {
            if (!FindNewTarget())
            {
                mainWeapon.StopFiring();
            }
        }
    }

    private bool CheckTargetIsValid()
    {
        //every 100 frames check that the current target is still a valid target
        if (validTargetCounter > 100)
        {
            validTargetCounter = 0;
            if (!enemyShips.Contains(currentTarget))
            {
                currentTarget = null;
            }
        }

        if (currentTarget != null)
        {
            return CheckLineOfSight(currentTarget.transform);
        }
        
        return currentTarget != null;
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        turretTower.LookAt(currentTarget.transform);
        turretTower.localRotation = new Quaternion(0, turretTower.localRotation.y, 0, turretTower.localRotation.w);
        turretGun.LookAt(currentTarget.transform);
    }

    private bool FindNewTarget()
    {
        //every 200 frames check for a new target
        //This function is expensive!
        if (targetSearchCounter < 200 || enemyShips.Count <= 0)
        {
            return false;
        }

        targetSearchCounter = 0;

        var shipsInRandomOrder = enemyShips.ToList();

        for (int i = 0; i < shipsInRandomOrder.Count/2; i++)
        {
            int rand = Random.Range(0, shipsInRandomOrder.Count-1);
            
            if (CheckLineOfSight(shipsInRandomOrder[rand].transform))
            {
                currentTarget = shipsInRandomOrder[rand];
                return true;
            }
            shipsInRandomOrder.RemoveAt(rand);
        }

        return false;
    }

    private bool CheckLineOfSight(Transform shipTransform)
    {
        if (Physics.Raycast(turretGun.position, shipTransform.position - turretGun.position, out hit, 150, layerMask))
        {
            FactionController factionController = hit.transform.gameObject.GetComponentInParent<FactionController>();
            if (factionController != null)
            {
                return factionId != factionController.factionID;
            }
        }

        return false;
    }

    public void SetEnemyShipCollection(HashSet<Collider> enemyShipsCollection)
    {
        enemyShips = enemyShipsCollection;
    }

    public void Destroyed()
    {
        mainWeapon.Destroyed();
    }
}