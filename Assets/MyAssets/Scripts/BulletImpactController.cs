using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactController : MonoBehaviour {
    public ParticleSystem ps;
    public Transform parent;
    private float delay;
    void Start()
    {
        delay = ps.main.duration;
    }

    public void Play()
    {
        StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        transform.parent = null;
        ps.Play();
        yield return new WaitForSeconds(delay);
        transform.parent = parent;
    }
}
