using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
    public int factionID;
    private List<GameObject> enemyShips = new List<GameObject>();
    private List<ShipController> friendlyShips = new List<ShipController>();
    private List<FactionController> otherFactions = new List<FactionController>();

    private void Awake()
    {
        GetShips();
    }

    void Start() {
        GetOtherFactions();

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
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            FactionController fc = ship.GetComponentInParent<FactionController>();
            if (fc != null && fc.factionID != factionID)
            {
                enemyShips.Add(ship);
            }
            else
            {
                friendlyShips.Add(ship.GetComponent<ShipController>());
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

    public void EnemyShipDestroyed(GameObject deadShip)
    {
        List<ShipController> shipsWithDeadTarget = new List<ShipController>();
        
        foreach (ShipController friendlyShip in friendlyShips)
        {
            if(friendlyShip.GetAiTarget() == deadShip)
            {
                shipsWithDeadTarget.Add(friendlyShip);
            }
            friendlyShip.RemoveEnemyTarget(deadShip);
        }

        enemyShips.Remove(deadShip);

        if(enemyShips.Count > 0)
        {
            foreach (ShipController ship in shipsWithDeadTarget)
            {
                ship.SetAiTarget(enemyShips[Random.Range(0, enemyShips.Count - 1)]);
            }
        }
        else
        {
            foreach (ShipController ship in shipsWithDeadTarget)
            {
                ship.SetAiTarget(null);
            }
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

    public List<ShipController> GetFriendlyShips()
    {
        return friendlyShips;
    }
}
