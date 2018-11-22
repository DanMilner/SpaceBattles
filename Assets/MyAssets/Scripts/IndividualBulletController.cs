using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBulletController : MonoBehaviour {
    public float lifeTime = 10.0f;

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter()
    {
        GameObject.Destroy(gameObject);
    }
}
