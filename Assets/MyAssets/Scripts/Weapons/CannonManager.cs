using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, IWeapon {
    [SerializeField] private GameObject cannonTarget;
    private CannonController[] Cannons;

    void Awake()
    {
        Cannons = gameObject.GetComponentsInChildren<CannonController>();
        ApplyCannonTarget();
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

    public void SetCannonTarget(GameObject target)
    {
        cannonTarget = target;
        ApplyCannonTarget();
    }

    private void ApplyCannonTarget()
    {
        for (int i = 0; i < Cannons.Length; i++)
        {
            Cannons[i].SetCannonTarget(cannonTarget);
        }
    }

    public void SetWeaponTarget(GameObject target)
    {
        cannonTarget = target;
    }
}
