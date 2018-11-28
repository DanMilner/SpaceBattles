using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretManager : MonoBehaviour {
    private int factionId;
    private AutoTurret[] autoTurrets;
    private HashSet<GameObject> enemyShips;

    void Start () {
        enemyShips = new HashSet<GameObject>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        autoTurrets = gameObject.GetComponentsInChildren<AutoTurret>();

        for (int i = 0; i < autoTurrets.Length; i++)
        {
            autoTurrets[i].SetEnenmyShipCollection(enemyShips);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().Faction)
            {
                enemyShips.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().Faction)
            {
                enemyShips.Remove(other.gameObject);
            }
        }
    }
}
