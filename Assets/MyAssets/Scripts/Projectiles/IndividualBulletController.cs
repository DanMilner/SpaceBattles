using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;
    public float damage = 1.0f;

    [SerializeField] private GameObject explosion;
    [SerializeField] private Rigidbody bulletRigidbody;

    private float counter = 0.0f;
    private float explosionLength = 1.0f;

    void Start()
    {
        if(explosion != null)
        {
            explosionLength = explosion.GetComponent<ParticleSystem>().main.duration;
        }
    }

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

            if (explosion != null)
            {
                GameObject explo = Instantiate(explosion, transform.position, transform.rotation, null);
                Destroy(explo, explosionLength);
            }

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
