using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;
    public ParticleSystem explosion;

    private float counter = 0.0f;
    private Transform parent;
    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;

        if (counter < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetParent(Transform p)
    {
        parent = p;
        transform.parent = p;
    }

    public void ResetBullet()
    {
        counter = lifeTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret") && !other.CompareTag("Bullet"))
        {
            explosion.transform.parent = null;
            explosion.Play();
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        transform.parent = parent;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
