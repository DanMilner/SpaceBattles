using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
    public int factionID;
    private List<GameObject> enemyShips = new List<GameObject>();
    private List<ShipController> friendlyShips = new List<ShipController>();
    private List<FactionController> otherFactions = new List<FactionController>();

    void Start() {
        GetOtherFactions();

        GetShips();

        SetEnemyShips();

        AssignRandomTargets();
    }
    
    private void SetEnemyShips()
    {
        foreach (ShipController ship in friendlyShips)
        {
            ship.SetEnemyShips(enemyShips);
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
            if (ship.GetComponentInParent<FactionController>().factionID != factionID)
            {
                enemyShips.Add(ship.transform.parent.gameObject);
            }
            else
            {
                friendlyShips.Add(ship.GetComponentInParent<ShipController>());
            }
        }
    }

    private void AssignRandomTargets()
    {
        if(enemyShips.Count <= 0)
        {
            return;
        }

        foreach (ShipController ship in friendlyShips)
        {
            ship.SetAiTarget(enemyShips[Random.Range(0, enemyShips.Count - 1)]);
        }
    }

    public void EnemyShipDestroyed(GameObject ship)
    {
        foreach (ShipController friendlyShip in friendlyShips)
        {
            friendlyShip.RemoveEnemyTarget(ship);
        }
    }

    public void FriendlyShipDestroyed(GameObject ship)
    {
        for (int i = 0; i < otherFactions.Count; i++)
        {
            otherFactions[i].EnemyShipDestroyed(ship);
        }

        ShipController shipController = ship.GetComponent<ShipController>();

        friendlyShips.Remove(shipController);
        shipController.DestroyShip();      
    }
}
