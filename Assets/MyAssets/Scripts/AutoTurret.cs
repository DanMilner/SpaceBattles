using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoTurretWeapon
{
    void Fire(Collider target);
}

public class AutoTurret : MonoBehaviour
{
    public float fireRate = 2.0f;
    public GameObject weapon;

    private IAutoTurretWeapon mainWeapon;
    private float cooldown;
    private HashSet<Collider> enemyShips;
    private Collider currentTarget;
    private int factionId;
    private RaycastHit hit;
    private int validTargetCounter = 0;
    private int lineOfSightCounter = 0;
    private int targetSearchCounter = 0;

    private int layerMask;

    // Use this for initialization
    void Start()
    {
        layerMask = ~(1 << 2);
        enemyShips = new HashSet<Collider>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        cooldown = fireRate;

        mainWeapon = weapon.GetComponent<IAutoTurretWeapon>();
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
        if (cooldown > 0) { return; }

        if (CheckTargetIsValid())
        {
            Fire();
            CheckTargetLineOfSight();
        }
        else
        {
            FindNewTarget();
        }
    }

    private void CheckTargetLineOfSight()
    {
        //every 20 frames check that the current target is still line of sight
        lineOfSightCounter++;
        if (lineOfSightCounter > 20)
        {
            if (!CheckLineOfShight(currentTarget.transform))
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
            gameObject.transform.LookAt(currentTarget.transform);
        }
    }

    private void FindNewTarget()
    {
        //every 200 frames check for a new target
        //This function is expensive!
        if(targetSearchCounter < 200) { return; }
        targetSearchCounter = 0;

        if (enemyShips.Count > 0)
        {
            foreach (Collider ship in enemyShips)
            {
                if (CheckLineOfShight(ship.transform))
                {
                    currentTarget = ship;
                    return;
                }
            }
        }
    }

    private bool CheckLineOfShight(Transform shipTransform)
    {
        if (Physics.Raycast(transform.position, shipTransform.position - transform.position, out hit, 15, layerMask))
        {
            return factionId != hit.transform.gameObject.GetComponentInParent<FactionID>().Faction;
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

    public void SetEnemyShipCollection(HashSet<Collider> enemyShipsCoolection)
    {
        enemyShips = enemyShipsCoolection;
    }
}
