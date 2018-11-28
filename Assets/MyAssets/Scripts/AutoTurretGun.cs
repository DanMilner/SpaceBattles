using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretGun : MonoBehaviour, IAutoTurretWeapon {
    public GameObject bulletPrefab;
    public GameObject spawnPoint;
    public float bulletLifeSpan;
    public float bulletSpeed;
    private int numBullets = 100;
    
    private Rigidbody ShipRigidbody;
    private Queue<GameObject> bullets;

    void Start () {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();        
        bullets = new Queue<GameObject>();
        Transform bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
        Vector3 spawnPosition = spawnPoint.transform.position;
        Quaternion spawnRotation = spawnPoint.transform.rotation;

        for (int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation, bulletHolder);            
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }        
    }

    public void Fire(GameObject target)
    {       
        GameObject bullet = bullets.Dequeue();

        if (!bullet.activeSelf)
        {
            Rigidbody bulletRigidBody = bullet.GetComponent<Rigidbody>();

            bullet.transform.position = spawnPoint.transform.position;
            bullet.transform.rotation = spawnPoint.transform.rotation;

            bullet.GetComponent<IndividualBulletController>().ResetBullet();
            bullet.SetActive(true);

            bulletRigidBody.velocity = ShipRigidbody.velocity;
            bulletRigidBody.AddForce(bullet.transform.forward * bulletSpeed);
        }
        bullets.Enqueue(bullet);
        
    }
}
