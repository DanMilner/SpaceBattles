using System.Collections.Generic;
using UnityEngine;

public interface IAutoTurretWeapon
{
    void Fire(Collider target);
    void StopFiring();
    void Destroyed();
}

public class AutoTurret : MonoBehaviour
{
    public float fireRate = 2.0f;

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

    void Awake()
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

        cooldown = fireRate;

        mainWeapon = GetComponent<IAutoTurretWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        LookAtTarget();
        validTargetCounter++;
        lineOfSightCounter++;
        targetSearchCounter++;

        //if gun cant shoot dont bother continuing.
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
        if (lineOfSightCounter > 20)
        {
            if (!CheckLineOfSight(currentTarget.transform))
            {
                currentTarget = null;
            }

            lineOfSightCounter = 0;
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

        return currentTarget != null;
    }

    private void LookAtTarget()
    {
        if (currentTarget != null)
        {
            turretTower.LookAt(currentTarget.transform);
            rotation = new Quaternion(0, turretTower.localRotation.y, 0, turretTower.localRotation.w);
            turretTower.localRotation = rotation;

            rotation = Quaternion.LookRotation(currentTarget.transform.position - turretGun.position, transform.up);
            turretGun.rotation = rotation;
        }
    }

    private void FindNewTarget()
    {
        //every 200 frames check for a new target
        //This function is expensive!
        if (targetSearchCounter < 200)
        {
            return;
        }

        targetSearchCounter = 0;

        if (enemyShips.Count > 0)
        {
            foreach (Collider ship in enemyShips)
            {
                if (CheckLineOfSight(ship.transform))
                {
                    currentTarget = ship;
                    return;
                }
            }
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
        if (cooldown < 0)
        {
            mainWeapon.Fire(currentTarget);
            cooldown = fireRate;
        }
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