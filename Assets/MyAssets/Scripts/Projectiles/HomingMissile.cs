using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {
    public float speed;
    public float turningRate;
    [SerializeField] private GameObject explosion;
    public ParticleSystem thruster;
    public Collider target;
    public float damage = 1.0f;
    public float lifeTime = 10.0f;
    private float timer;

    private Rigidbody missileRigidBody;
    private float explosionLength = 1.0f;

    void Start () {
        missileRigidBody = gameObject.GetComponent<Rigidbody>();
        thruster.Play();
        Reset();

        if (explosion != null)
        {
            explosionLength = explosion.GetComponent<ParticleSystem>().main.duration;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            Detonate();
        }

        missileRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation,
                            Quaternion.LookRotation(target.transform.position - transform.position),
                            turningRate));

        missileRigidBody.MovePosition(missileRigidBody.position + transform.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Collider target)
    {
        this.target = target;
        Reset();
    }

    public void Reset()
    {
        timer = lifeTime;
        thruster.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret"))
        {
            ShipHealth shipHealth = other.gameObject.GetComponentInParent<ShipHealth>();
            if(shipHealth != null)
            {
                shipHealth.TakeDamage(damage);
            }
            Detonate();
        }
    }

    private void Detonate()
    {
        thruster.Stop();

        if (explosion != null)
        {
            GameObject explo = Instantiate(explosion, transform.position, transform.rotation, null);
            Destroy(explo, explosionLength);
        }

        gameObject.SetActive(false);
    }
}
