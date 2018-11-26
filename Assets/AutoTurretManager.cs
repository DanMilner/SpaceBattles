using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretManager : MonoBehaviour {
    private HashSet<GameObject> EnemyShips;
    private int factionId;

    private AutoTurret[] autoTurrets;

    // Use this for initialization
    void Start () {
        EnemyShips = new HashSet<GameObject>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        autoTurrets = gameObject.GetComponentsInChildren<AutoTurret>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().Faction)
            {
                for(int i = 0; i < autoTurrets.Length; i++)
                {
                    autoTurrets[i].AddShip(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().Faction)
            {
                for (int i = 0; i < autoTurrets.Length; i++)
                {
                    autoTurrets[i].RemoveShip(other.gameObject);
                }
            }
        }
    }
}
