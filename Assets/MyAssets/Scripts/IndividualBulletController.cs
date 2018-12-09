using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;
    public BulletImpactController explosion;
    public float damage = 1.0f;

    private float counter = 0.0f;
    public Rigidbody bulletRigidbody;
    
    void Update()
    {
        counter -= Time.deltaTime;

        if (counter < 0)
        {
            gameObject.SetActive(false);
        }
    }    

    public void Fire(Transform spawnPoint, Vector3 velocity, float speed)
    {
        counter = lifeTime;

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        bulletRigidbody.velocity = velocity;
        bulletRigidbody.AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet"))
        {
            if (other.CompareTag("AutoTurret"))
            {
                TurretHealth turretHealth = other.gameObject.GetComponentInChildren<TurretHealth>();
                if (turretHealth != null)
                {
                    turretHealth.TakeDamage(damage);
                }
            }
            else if (other.CompareTag("ShipCollider"))
            {
                ShipHealth shipHealth = other.gameObject.GetComponentInParent<ShipHealth>();
                if (shipHealth != null)
                {
                    shipHealth.TakeDamage(damage);
                }
            }

            explosion.Play();
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        bulletRigidbody.velocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
