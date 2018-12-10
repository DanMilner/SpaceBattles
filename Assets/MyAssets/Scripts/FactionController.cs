using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
    public int factionID;
    private List<GameObject> enemyShips = new List<GameObject>();
    private List<ShipAI> friendlyShips = new List<ShipAI>();
    private List<FactionController> otherFactions = new List<FactionController>();

    // Use this for initialization
    void OnEnable () {
        SetShipFactionIds();

        SetEnemyShips();

        GetOtherFactions();

        GetShips();

        AssignRandomTargets();
    }

    private void SetShipFactionIds()
    {
        FactionID[] factionIDs = gameObject.GetComponentsInChildren<FactionID>();

        for (int i = 0; i < factionIDs.Length; i++)
        {
            factionIDs[i].factionID = factionID;
        }
    }

    private void SetEnemyShips()
    {
        AutoTurretManager[] autoTurretManagers = gameObject.GetComponentsInChildren<AutoTurretManager>();

        for (int i = 0; i < autoTurretManagers.Length; i++)
        {
            autoTurretManagers[i].SetEnemyShips(enemyShips);
        }
    }

    private void GetOtherFactions()
    {
        foreach (GameObject faction in GameObject.FindGameObjectsWithTag("Faction"))
        {
            otherFactions.Add(faction.GetComponent<FactionController>());
        }
    }

    private void GetShips()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("ShipTarget"))
        {
            if (ship.GetComponentInParent<FactionID>().factionID != factionID)
            {
                enemyShips.Add(ship.transform.parent.gameObject);
            }
            else
            {
                friendlyShips.Add(ship.GetComponentInParent<ShipAI>());
            }
        }
    }

    private void AssignRandomTargets()
    {
        if(enemyShips.Count <= 0)
        {
            return;
        }

        foreach (ShipAI ai in friendlyShips)
        {
            ai.target = enemyShips[Random.Range(0, enemyShips.Count - 1)];
        }
    }

    public void EnemyShipDestroyed(GameObject ship)
    {
        AutoTurretManager[] autoTurretManagers = gameObject.GetComponentsInChildren<AutoTurretManager>();
        
        for (int i = 0; i < autoTurretManagers.Length; i++)
        {
            autoTurretManagers[i].RemoveEnemyShip(ship);
        }
    }

    public void FriendlyShipDestroyed(GameObject ship)
    {
        for (int i = 0; i < otherFactions.Count; i++)
        {
            otherFactions[i].EnemyShipDestroyed(ship);
        }

        friendlyShips.Remove(ship.GetComponent<ShipAI>());

        MonoBehaviour[] scripts = ship.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour m in scripts)
        {
            m.enabled = false;
        }        
    }
}
