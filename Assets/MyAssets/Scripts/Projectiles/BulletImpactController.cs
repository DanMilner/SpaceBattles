using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactController : MonoBehaviour {
    [SerializeField] private ParticleSystem ps;
    
    public void Play(Vector3 newPosition)
    {
        transform.position = newPosition;
        ps.Play();
    }
}
