using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretMissiles : MonoBehaviour {
    public float range = 500f;
    public SphereCollider rangeCollider;

    public GameObject missilePrefab;
    public GameObject spawnPoint;
    public float fireRate = 2.0f;
    private float cooldown;
    private Rigidbody ShipRigidbody;

    private HashSet<GameObject> EnemyShips;
    private GameObject currentTarget;
    private int factionId;
    
	// Use this for initialization
	void Start () {
        rangeCollider.radius = range;
        EnemyShips = new HashSet<GameObject>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        cooldown = fireRate;
    }

    // Update is called once per frame
    void Update () {
        cooldown -= Time.deltaTime;

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
        if(EnemyShips.Count > 0)
        {
            foreach(GameObject ship in EnemyShips)
            {
                if(ship == null)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if(factionId != other.gameObject.GetComponentInParent<FactionID>().Faction)
            {
                EnemyShips.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyShips.Remove(other.gameObject);
    }

    private void Fire()
    {
        if (cooldown < 0)
        {
            ShootMissile();
            cooldown = fireRate;
        }
    }

    private void ShootMissile()
    {
        GameObject missile = Instantiate(missilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        missile.GetComponent<HomingMissile>().SetTarget(currentTarget);
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
    }
}
