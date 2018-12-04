﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretManager : MonoBehaviour {
    private int factionId;
    private AutoTurret[] autoTurrets;
    private HashSet<GameObject> enemyShips;
    private int count = 0;
    void Start () {
        enemyShips = new HashSet<GameObject>();
        factionId = gameObject.GetComponentInParent<FactionID>().Faction;
        autoTurrets = gameObject.GetComponentsInChildren<AutoTurret>();

        for (int i = 0; i < autoTurrets.Length; i++)
        {
            autoTurrets[i].SetEnenmyShipCollection(enemyShips);
        }
    }

    void Update()
    {
        count++;
        //every  60 frames check that the list of targets are still alive
        if(count > 60)
        {
            count = 0;

            List<GameObject> shipsToRemove = new List<GameObject>();
            foreach (GameObject ship in enemyShips)
            {
                if (!ship.GetComponentInParent<ShipHealth>().alive)
                {
                    shipsToRemove.Add(ship);
                }
            }

            if(shipsToRemove.Count > 0)
            {
                for(int i = 0; i < shipsToRemove.Count; i++)
                {
                    enemyShips.Remove(shipsToRemove[i]);
                }
            }
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
                Debug.Log("Removing ship ");
                enemyShips.Remove(other.gameObject);
            }
        }
    }
}
