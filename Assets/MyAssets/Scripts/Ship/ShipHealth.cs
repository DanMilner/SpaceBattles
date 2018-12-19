using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour {
    public float health = 100.0f;
    public bool alive = true;

    [SerializeField] private GameObject deathExplostion;
    [SerializeField] private GameObject AutoTurrets;
    [SerializeField] private GameObject damagePoints;
    [SerializeField] private Material deadMaterial;
    [SerializeField] private MeshRenderer[] meshes;

    private ParticleSystem[] damage;
    private UIHandler uIHandler;
    private int damageActive = 0;
    private float damageThreshold;
    private float currentThreshold;
    private bool isPlayerShip;

    void Awake()
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
            if(uIHandler == null)
            {
                uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
            }
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
        foreach(MeshRenderer renderer in meshes)
        {
            renderer.material = deadMaterial;
        }

        Instantiate(deathExplostion, transform.position, transform.rotation);

        InformFactionControllerOfDeath();
    }

    private void InformFactionControllerOfDeath()
    {
        GetComponentInParent<FactionController>().FriendlyShipDestroyed(gameObject.transform.parent.gameObject);
    }
}
