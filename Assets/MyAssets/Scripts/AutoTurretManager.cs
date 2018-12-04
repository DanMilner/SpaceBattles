using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretManager : MonoBehaviour {
    private int factionId;
    private AutoTurret[] autoTurrets;
    private HashSet<Collider> enemyShips;

    void Start () {
        enemyShips = new HashSet<Collider>();
        factionId = gameObject.GetComponentInParent<FactionID>().factionID;
        autoTurrets = gameObject.GetComponentsInChildren<AutoTurret>();

        for (int i = 0; i < autoTurrets.Length; i++)
        {
            autoTurrets[i].SetEnemyShipCollection(enemyShips);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().factionID)
            {
                ShipHealth shipHealth = other.gameObject.GetComponentInParent<ShipHealth>();
                if (shipHealth.alive)
                {
                    shipHealth.SetTargeted(this);
                    enemyShips.Add(other);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShipCollider"))
        {
            if (factionId != other.gameObject.GetComponentInParent<FactionID>().factionID)
            {
                enemyShips.Remove(other);
                other.gameObject.GetComponentInParent<ShipHealth>().SetNotTargeted(this);
            }
        }
    }

    public void RemoveShip(Collider collider)
    {
        enemyShips.Remove(collider);
    }

    public void RemoveShips(Collider[] collider)
    {
        for(int i = 0; i < collider.Length; i++)
        {
            enemyShips.Remove(collider[i]);
        }
    }
}
