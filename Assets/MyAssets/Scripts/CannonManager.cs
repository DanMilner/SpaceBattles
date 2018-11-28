using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, IWeapon {
    public CannonController[] Cannons;
    
    public void Fire()
    {
        for(int i =0; i < Cannons.Length; i++)
        {
            Cannons[i].FireCannon();
        }
    }

    public string GetName()
    {
        return "Cannons";
    }
}
