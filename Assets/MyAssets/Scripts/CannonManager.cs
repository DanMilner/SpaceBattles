using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, IWeapon {
    public GameObject cannonTarget;
    private CannonController[] Cannons;

    void Start()
    {
        Cannons = gameObject.GetComponentsInChildren<CannonController>();
        for (int i = 0; i < Cannons.Length; i++)
        {
            Cannons[i].SetCannonTarget(cannonTarget);
        }
    }

    public void Fire()
    {
        for(int i = 0; i < Cannons.Length; i++)
        {
            Cannons[i].FireCannon();
        }
    }

    public string GetName()
    {
        return "Cannons";
    }
}
