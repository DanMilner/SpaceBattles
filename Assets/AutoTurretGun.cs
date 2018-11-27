using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretGun : MonoBehaviour, IAutoTurretWeapon {
    public GameObject bulletPrefab;
    public GameObject spawnPoint;
    public float bulletLifeSpan;
    public float bulletSpeed;
    private int numBullets = 40;

    private Rigidbody ShipRigidbody;
    private Queue<GameObject> bullets;

    void Start () {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();        
        bullets = new Queue<GameObject>();
        for(int i = 0; i < numBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);            
            bullet.SetActive(false);
            bullet.GetComponent<IndividualBulletController>().SetParent(transform);
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

            bullet.transform.parent = null;
        }
        bullets.Enqueue(bullet);
        
    }
}
