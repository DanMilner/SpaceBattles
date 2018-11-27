using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoTurretWeapon
{
    void Fire(GameObject target);
}

public class AutoTurret : MonoBehaviour
{
    public float fireRate = 2.0f;
    public GameObject weapon;

    private IAutoTurretWeapon mainWeapon;
    private float cooldown;
    private HashSet<GameObject> EnemyShips;
    private GameObject currentTarget;
    private int factionId;

    // Use this for initialization
    void Start()
    {
        EnemyShips = new HashSet<GameObject>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        cooldown = fireRate;

        mainWeapon = weapon.GetComponent<IAutoTurretWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if(cooldown > 0)
        {
            if(currentTarget != null)
            {
                gameObject.transform.LookAt(currentTarget.transform);
            }
            return;
        }

        if (currentTarget == null)
        {
            FindNewTarget();
        }
        else
        {
            if (CheckLineOfShight(currentTarget.transform))
            {
                gameObject.transform.LookAt(currentTarget.transform);
                Fire();
            }
            else
            {
                currentTarget = null;
            }
        }
    }

    private void FindNewTarget()
    {
        if (EnemyShips.Count > 0)
        {
            foreach (GameObject ship in EnemyShips)
            {
                if (ship == null)
                {
                    EnemyShips.Remove(ship);
                }

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
        // ignore layer 2
        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        var heading = shipTransform.position - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, heading, out hit, 15, layerMask))
        {
            return factionId != hit.transform.gameObject.GetComponentInParent<FactionID>().Faction;
        }
        return false;
    }

    public void AddShip(GameObject enemyShip)
    {
        EnemyShips.Add(enemyShip);
    }

    public void RemoveShip(GameObject enemyShip)
    {
        EnemyShips.Remove(enemyShip);
    }

    private void Fire()
    {
        if (cooldown < 0)
        {
            mainWeapon.Fire(currentTarget);
            cooldown = fireRate;
        }
    }
}
