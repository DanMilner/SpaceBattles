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
    private Quaternion rotation;
    private float cooldown;
    private int factionId;
    private int validTargetCounter;
    private int lineOfSightCounter;
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

        cooldown = FireRate;

        mainWeapon = GetComponent<IAutoTurretWeapon>();
    }

    // Update is called once per frame
    private void Update()
    {
        cooldown -= Time.deltaTime;

        LookAtTarget();
        validTargetCounter++;
        lineOfSightCounter++;
        targetSearchCounter++;

        //if gun cant shoot don't bother continuing.
        if (cooldown > 0)
        {
            return;
        }

        if (CheckTargetIsValid())
        {
            Fire();
            CheckTargetLineOfSight();
        }
        else
        {
            FindNewTarget();
        }

        if (currentTarget == null)
        {
            mainWeapon.StopFiring();
        }
    }

    private void CheckTargetLineOfSight()
    {
        //every 20 frames check that the current target is still line of sight
        lineOfSightCounter++;
        if (lineOfSightCounter <= 20)
        {
            return;
        }

        if (!CheckLineOfSight(currentTarget.transform))
        {
            currentTarget = null;
        }

        lineOfSightCounter = 0;
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

        return currentTarget != null;
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        turretTower.LookAt(currentTarget.transform);
        rotation = new Quaternion(0, turretTower.localRotation.y, 0, turretTower.localRotation.w);
        turretTower.localRotation = rotation;

        rotation = Quaternion.LookRotation(currentTarget.transform.position - turretGun.position, transform.up);
        turretGun.rotation = rotation;
    }

    private void FindNewTarget()
    {
        //every 200 frames check for a new target
        //This function is expensive!
        if (targetSearchCounter < 200 || enemyShips.Count <= 0)
        {
            return;
        }

        targetSearchCounter = 0;

        var shipsInRandomOrder = enemyShips.ToList();

        for (int i = 0; i < shipsInRandomOrder.Count; i++)
        {
            int rand = Random.Range(0, shipsInRandomOrder.Count-1);
            
            if (CheckLineOfSight(shipsInRandomOrder[rand].transform))
            {
                currentTarget = shipsInRandomOrder[rand];
                return;
            }
            shipsInRandomOrder.RemoveAt(rand);
        }
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

    private void Fire()
    {
        if (cooldown > 0)
        {
            return;
        }

        mainWeapon.Fire(currentTarget);
        cooldown = FireRate;
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