using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
    public float bulletSpeed = 200.0f;
    public float fireRate = 2.0f;
    public float bulletLifeSpan = 10.0f;
    private float bulletCooldown;
    private Rigidbody ShipRigidbody;

    public void Start()
    {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    public void Update()
    {
        bulletCooldown -= Time.deltaTime;
    }

    public void Fire()
    {
        if (bulletCooldown < 0)
        {
            ShootBullet();
            bulletCooldown = fireRate;
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.parent = null;
        bullet.GetComponent<IndividualBulletController>().Fire(bulletSpawn.transform, ShipRigidbody.velocity, bulletSpeed);

        Destroy(bullet, bulletLifeSpan);
    }

    public string GetName()
    {
        return "Gun";
    }

    public void SetWeaponTarget(GameObject target)
    {
        //TODO: implement weapon target. Gun should face where the player is looking.
        return;
    }
}
