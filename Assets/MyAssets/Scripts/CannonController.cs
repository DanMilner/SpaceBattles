using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    public float coolDownTime = 10.0f;
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float bulletLifeSpan = 10.0f;
    public Transform cannonTower;
    public Transform cannonGun;
    private Transform cannonTarget;

    private float coolDown;
    private Rigidbody shipRigidbody;
    private Quaternion rotation;
    private LineOfSight lineOfSight;

    void Start()
    {
        shipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        coolDown = coolDownTime;
        lineOfSight = cannonGun.GetComponent<LineOfSight>();
    }

    void Update()
    {
        coolDown -= Time.deltaTime;

        cannonTower.LookAt(cannonTarget);
        rotation = new Quaternion(0, cannonTower.localRotation.y, 0, cannonTower.localRotation.w);
        cannonTower.localRotation = rotation;
       
        rotation = Quaternion.LookRotation(cannonTarget.position - cannonGun.position, transform.up);
        cannonGun.rotation = rotation;
    }

    public void FireCannon()
    {
        if (lineOfSight.SafeToFire())
        {
            if (coolDown <= 0)
            {
                coolDown = coolDownTime;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                bullet.transform.parent = null;
                bullet.GetComponent<IndividualBulletController>().Fire(bulletSpawn.transform, shipRigidbody.velocity, bulletSpeed);

                Destroy(bullet, bulletLifeSpan);
            }
        }
    }

    public void SetCannonTarget(GameObject target)
    {
        cannonTarget = target.transform;
    }
}
