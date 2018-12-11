using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactController : MonoBehaviour {
    public ParticleSystem ps;

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("BulletHolder").transform;
    }
    
    public void Play(Vector3 newPosition)
    {
        transform.position = newPosition;
        ps.Play();
    }
}
