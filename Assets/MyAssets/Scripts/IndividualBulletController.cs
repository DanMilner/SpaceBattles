using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;
    public BulletImpactController explosion;
    public float damage = 1.0f;

    private float counter = 0.0f;

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;

        if (counter < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetBullet()
    {
        counter = lifeTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret") && !other.CompareTag("Bullet"))
        {
            ShipHealth shipHealth = other.gameObject.GetComponentInParent<ShipHealth>();
            if (shipHealth != null)
            {
                shipHealth.TakeDamage(damage);
            }
            explosion.Play();
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
