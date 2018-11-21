using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public float missileSpeed = 50.0f;
    public GameObject[] Missiles;

    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
    public float bulletSpeed = 100.0f;
    public float fireRate = 2.0f;
    public float bulletLifeSpan = 10.0f;
    private float bulletCooldown;

    private int missilesLeft;
    private Rigidbody ShipRigidbody;

    // Use this for initialization
    void Start () {
        missilesLeft = Missiles.Length - 1;
        ShipRigidbody = gameObject.GetComponent<Rigidbody>();
        bulletCooldown = fireRate;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (missilesLeft >= 0) { ShootMissile(); }
        }

        bulletCooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (bulletCooldown < 0) {
                ShootBullet();
                bulletCooldown = fireRate;
            }
        }
    }

    private void ShootMissile()
    {
        GameObject missile = Missiles[missilesLeft--];
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * missileSpeed);
        missile.GetComponentInChildren<MissileController>().enabled = true;
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.transform.parent = null;
        bullet.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);

        Destroy(bullet, bulletLifeSpan);
    }
}
