using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour {
    public float health = 1.0f;

    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private ParticleSystem damagedParticles;
    private float damagedThreshold;
    private ParticleSystem particles;

    void Start()
    {
        damagedThreshold = health / 2;
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0) { return; }
        
        health -= damage;

        if(particles == null && health <= damagedThreshold)
        {
            particles = Instantiate(damagedParticles, transform.position, Quaternion.identity, transform);
        }

        if (health <= 0)
        {
            DestroyTurret();
        }        
    }

    private void DestroyTurret()
    {
        Destroy(particles);
        particles = Instantiate(destroyedParticles, transform.position, Quaternion.identity, transform);

        gameObject.GetComponent<AutoTurret>().enabled = false;
        gameObject.GetComponent<TurretHealth>().enabled = false;       
    }
}
