using UnityEngine;

public class CannonManager : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject cannonTarget;
    private CannonController[] cannons;

    private void Awake()
    {
        cannons = gameObject.GetComponentsInChildren<CannonController>();
        ApplyCannonTarget();
    }

    public void Fire()
    {
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].FireCannon();
        }
    }

    public string GetName()
    {
        return "Cannons";
    }

    private void ApplyCannonTarget()
    {
        for (int i = 0; i < cannons.Length; i++)
        {
            cannons[i].SetCannonTarget(cannonTarget);
        }
    }

    public void SetWeaponTarget(GameObject target)
    {
        cannonTarget = target;

        if (target == null)
        {
            return;
        }

        ApplyCannonTarget();
    }
}