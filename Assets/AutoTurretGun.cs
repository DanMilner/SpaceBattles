using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretGun : MonoBehaviour, IAutoTurretWeapon {
    public GameObject bulletPrefab;
    public GameObject spawnPoint;
    public float bulletLifeSpan;
    public float bulletSpeed;

    private Rigidbody ShipRigidbody;

    void Start () {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    public void Fire(GameObject target)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        bullet.transform.parent = null;
        bullet.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);

        Destroy(bullet, bulletLifeSpan);
    }
}
