using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {
    public float speed;
    public float turningRate;
    public ParticleSystem explosion;
    public ParticleSystem thruster;
    public GameObject target;

    public float lifeTime = 10.0f;

    private Rigidbody missileRigidBody;

    // Use this for initialization
    void Start () {
        missileRigidBody = gameObject.GetComponent<Rigidbody>();
        thruster.Play();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        missileRigidBody.velocity = transform.forward * speed;

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        missileRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate));
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret"))
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }
}
