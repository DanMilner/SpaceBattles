using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretMissiles : MonoBehaviour {
    public float range = 500f;
    public SphereCollider rangeCollider;
    private IWeapon gun;
    public GameObject Weapons;

    private HashSet<GameObject> EnemyShips;
    private GameObject currentTarget;
    private int factionId;
    
	// Use this for initialization
	void Start () {
        rangeCollider.radius = range;
        EnemyShips = new HashSet<GameObject>();
        gun = Weapons.GetComponent<IWeapon>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
    }

    // Update is called once per frame
    void Update () {
        if (currentTarget == null)
        {
            FindNewTarget();
        }
        else
        {
            if (CheckLineOfShight(currentTarget.transform))
            {
                gameObject.transform.LookAt(currentTarget.transform);
                gun.Fire();
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

                //Debug.Log(ship.tag);

                if (CheckLineOfShight(ship.transform))
                {
                    currentTarget = ship;
                    return;
                }
            }
            //Debug.Log("-----------------------");

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
            //return hit.transform.CompareTag("Player");
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
}
