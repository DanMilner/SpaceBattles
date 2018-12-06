using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactController : MonoBehaviour {
    public ParticleSystem ps;
    public Transform parent;
    private float delay;
    private float time = 0.0f;

    void Start()
    {
        delay = ps.main.duration;
    }

    public void Play()
    {
        time = delay;
        transform.parent = null;
        ps.Play();
    }

    void Update()
    {
        time -= Time.deltaTime;

        if(time < 0)
        {
            transform.parent = parent;
        }
    }
}
