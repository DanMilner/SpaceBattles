using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour {
    public float health = 100.0f;
    public GameObject deathExplostion;
    public GameObject AutoTurrets;
    public GameObject damagePoints;
    public bool alive = true;
    public Material deadMaterial;

    private ParticleSystem[] damage;
    private UIHandler uIHandler;
    int damageActive = 0;
    float damageThreshold;
    float currentThreshold;
    private bool isPlayerShip;

    void Start()
    {
        uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
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

        UpdateUi();
    }

    public void SetPlayerControlled(bool pControlled)
    {
        isPlayerShip = pControlled;
        UpdateUi();
    }

    private void UpdateUi()
    {
        if (isPlayerShip)
        {
            uIHandler.SetCurrentHealth(health);
        }
    }

    private void StartFire()
    {
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

        foreach(MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = deadMaterial;
        }

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
