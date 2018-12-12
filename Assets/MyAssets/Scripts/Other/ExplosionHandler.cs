using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour {
    private ParticleSystem ps;

	void Awake () {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    void Update () {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
