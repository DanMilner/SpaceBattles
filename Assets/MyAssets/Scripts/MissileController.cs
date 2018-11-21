using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {
    public float accelerationSpeed = 1.0f;
    public float lifeTime = 10.0f;

    public ParticleSystem explosion;
    public ParticleSystem thruster;

    private float armingTimer = 0.50f;

    private Rigidbody rb;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        thruster.Play();
    }
	
	void FixedUpdate () {
        rb.AddForce(gameObject.transform.forward * accelerationSpeed);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        armingTimer -= Time.deltaTime;

        if(armingTimer < 0)
        {
            gameObject.GetComponentInChildren<Collider>().enabled = true;
        }

        if (lifeTime < 0)
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter()
    {
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
