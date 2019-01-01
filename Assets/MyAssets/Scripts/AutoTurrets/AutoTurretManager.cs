using System.Collections.Generic;
using UnityEngine;

public class AutoTurretManager : MonoBehaviour {
    public float range = 400.0f;

    private AutoTurret[] autoTurrets;
    private HashSet<Collider> enemyTargets;
    private List<GameObject> enemyShips;
    private HashSet<GameObject> nearbyShips;
    private int count;

    private void Awake()
    {
        enemyTargets = new HashSet<Collider>();
        nearbyShips = new HashSet<GameObject>();
        autoTurrets = gameObject.GetComponentsInChildren<AutoTurret>();
    }

    private void Start () {
        for (int i = 0; i < autoTurrets.Length; i++)
        {
            autoTurrets[i].SetEnemyShipCollection(enemyTargets);
        }
    }

    public void SetEnemyShips(List<GameObject> ships)
    {
        enemyShips = ships;
    }

    private void Update()
    {
        count++;
        if(count > 100)
        {
            GetNearbyShips();
            count = 0;
        }
    }

    private void GetNearbyShips()
    {
        if(enemyShips == null) { return; }

        for(int i = 0; i < enemyShips.Count; i++)
        {
            if (Vector3.Distance(enemyShips[i].transform.position, transform.position) <= range)
            {
                if (!nearbyShips.Contains(enemyShips[i]))
                {
                    nearbyShips.Add(enemyShips[i]);

                    AddTargets(enemyShips[i].GetComponentsInChildren<Collider>());
                }
            }
            else
            {
                if (nearbyShips.Contains(enemyShips[i]))
                {
                    nearbyShips.Remove(enemyShips[i]);
                    RemoveTargets(enemyShips[i].GetComponentsInChildren<Collider>());
                }
            }
        }        
    }

    public void RemoveEnemyShip(GameObject ship)
    {
        enemyShips.Remove(ship);
        RemoveTargets(ship.GetComponentsInChildren<Collider>());
    }

    public void RemoveTargets(Collider[] collider)
    {
        for(int i = 0; i < collider.Length; i++)
        {
            enemyTargets.Remove(collider[i]);
        }
    }

    public void AddTargets(Collider[] collider)
    {
        for (int i = 0; i < collider.Length; i++)
        {
            enemyTargets.Add(collider[i]);
        }
    }

    public void ShipDestroyed()
    {
        for (int i = 0; i < autoTurrets.Length; i++)
        {
            autoTurrets[i].Destroyed();
        }
    }
}
