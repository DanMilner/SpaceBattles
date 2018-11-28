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
    private HashSet<GameObject> enemyShips;
    private GameObject currentTarget;
    private int factionId;
    private RaycastHit hit;
    int layerMask;

    // Use this for initialization
    void Start()
    {
        layerMask = ~(1 << 2);
        enemyShips = new HashSet<GameObject>();
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
        if (enemyShips.Count > 0)
        {
            foreach (GameObject ship in enemyShips)
            {
                if (ship == null)
                {
                    enemyShips.Remove(ship);
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

    public void SetEnenmyShipCollection(HashSet<GameObject> enemyShipsCoolection)
    {
        enemyShips = enemyShipsCoolection;
    }
}
