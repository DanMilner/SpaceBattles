using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
    public float bulletSpeed = 100.0f;
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
        bullet.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
        bullet.GetComponent<IndividualBulletController>().ResetBullet();

        Destroy(bullet, bulletLifeSpan);
    }

    public string GetName()
    {
        return "Gun";
    }
 }
