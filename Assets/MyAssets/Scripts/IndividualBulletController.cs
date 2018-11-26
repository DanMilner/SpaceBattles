using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;
    public ParticleSystem explosion;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret") && !other.CompareTag("Bullet"))
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }
}
