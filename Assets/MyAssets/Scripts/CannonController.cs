using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    public float coolDownTime = 10.0f;
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float bulletLifeSpan = 10.0f;

    private float coolDown;
    private Rigidbody shipRigidbody;

    void Start()
    {
        shipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        coolDown = coolDownTime;
    }

    void Update()
    {
        coolDown -= Time.deltaTime;
    }

    public void FireCannon()
    {
        if(coolDown <= 0)
        {
            coolDown = coolDownTime;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.transform.parent = null;
            bullet.GetComponent<Rigidbody>().velocity = shipRigidbody.velocity;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);
            bullet.GetComponent<IndividualBulletController>().ResetBullet();

            Destroy(bullet, bulletLifeSpan);
        }
    }
}
