using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour {
    public float health = 100.0f;
    public GameObject deathExplostion;
    public GameObject fire;
    public GameObject AutoTurrets;
    public GameObject damagePoints;

    private bool alive = true;
    private ParticleSystem[] damage;
    int damageActive = 0;
    float damageThreshold;
    float currentThreshold;

    void Start()
    {
        damage = damagePoints.GetComponentsInChildren<ParticleSystem>();

        damageThreshold = health / damage.Length;
        currentThreshold = damageThreshold;
    }
    public void TakeDamage(float damage)
    {
        if (!alive)
        {
            return;
        }

        health -= damage;

        currentThreshold -= damage;

        while (currentThreshold <= 0)
        {
            damage = currentThreshold * -1;
            StartFire();
            currentThreshold = damageThreshold;
            currentThreshold -= damage;
        }

        if (health < 0)
        {
            alive = false;
            DestroyShip();
        }
    }

    private void StartFire()
    {
        Debug.Log("Fire started");
        if(damageActive < damage.Length)
        {
            damage[damageActive++].Play();
        }
    }

    private void DestroyShip()
    {
        gameObject.GetComponent<FlightController>().enabled = false;
        gameObject.GetComponent<ThrusterParticlesController>().enabled = false;
        gameObject.GetComponent<WeaponController>().enabled = false;

        DisableAutoTurrets();

        Instantiate(deathExplostion, transform.position, transform.rotation);
    }

    private void DisableAutoTurrets()
    {
        if(AutoTurrets != null)
        {
            foreach (AutoTurret autoTurret in AutoTurrets.GetComponentsInChildren<AutoTurret>())
            {
                autoTurret.enabled = false;
            }
        }
    }
}
