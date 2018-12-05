using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
    public int factionID;
    private List<GameObject> enemyShips = new List<GameObject>();
    private List<ShipAI> friendlyShips = new List<ShipAI>();

    // Use this for initialization
    void OnEnable () {
        SetShipFactionIds();

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

    private void GetShips()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            if (ship.GetComponentInParent<FactionID>().factionID != factionID)
            {
                enemyShips.Add(ship);
            }
            else
            {
                friendlyShips.Add(ship.GetComponentInParent<ShipAI>());
            }
        }
    }

    private void AssignRandomTargets()
    {
        foreach (ShipAI ai in friendlyShips)
        {
            ai.target = enemyShips[Random.Range(0, enemyShips.Count - 1)];
        }
    }
}
