using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretGun : MonoBehaviour, IAutoTurretWeapon {
    public float bulletLifeSpan = 10.0f;
    public float bulletSpeed = 1.0f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;

    private int numBullets = 120;    
    private Rigidbody ShipRigidbody;
    private Queue<GameObject> bullets;
    private Rigidbody bulletRigidBody;
    private GameObject bullet;

    void Start () {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();        
        bullets = new Queue<GameObject>();
        Transform bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
        Vector3 spawnPosition = spawnPoint.position;
        Quaternion spawnRotation = spawnPoint.rotation;

        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation, bulletHolder);            
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }        
    }

    public void Fire(Collider target)
    {       
        bullet = bullets.Dequeue();

        if (!bullet.activeSelf)
        {
            bullet.SetActive(true);
            bullet.GetComponent<IndividualBulletController>().Fire(spawnPoint, ShipRigidbody.velocity, bulletSpeed);
        }
        bullets.Enqueue(bullet);        
    }
}
