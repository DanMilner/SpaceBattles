using UnityEngine;

public class AutoTurretGun : MonoBehaviour, IAutoTurretWeapon {
    [SerializeField] private ParticleSystem machineGun;
    private bool isFiring;

    public void Fire(Collider target)
    {       
        if(isFiring) { return; }

        machineGun.Play();
        isFiring = true;
    }

    public void StopFiring()
    {
        machineGun.Stop();
        isFiring = false;
    }

    public void Destroyed()
    {
        machineGun.Stop();
    }
}