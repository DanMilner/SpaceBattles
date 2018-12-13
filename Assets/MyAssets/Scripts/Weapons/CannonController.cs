using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {
    public float coolDownTime = 10.0f;
    public float bulletSpeed = 10.0f;
    public float bulletLifeSpan = 10.0f;

    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cannonTower;
    [SerializeField] private Transform cannonGun;

    private Transform cannonTarget;
    private float coolDown;
    private Rigidbody shipRigidbody;
    private Quaternion rotation;
    private LineOfSight lineOfSight;
    private Transform bulletHolder;

    void Awake()
    {
        shipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
        coolDown = coolDownTime;
        lineOfSight = cannonGun.GetComponent<LineOfSight>();
        bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
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
        if (coolDown <= 0)
        {
            if (lineOfSight.SafeToFire())
            {
                coolDown = coolDownTime;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bulletHolder);
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