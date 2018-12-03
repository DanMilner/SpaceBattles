using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactController : MonoBehaviour {
    public ParticleSystem ps;
    public Transform parent;

    public void Play()
    {
        StartCoroutine(Coroutine());
    }

    public IEnumerator Coroutine()
    {
        transform.parent = null;
        ps.Play();
        yield return new WaitForSeconds(0.2f);
        transform.parent = parent;
    }
}
