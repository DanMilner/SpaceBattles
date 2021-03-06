﻿using System.Collections.Generic;
using UnityEngine;

public class AutoTurretMissiles : MonoBehaviour, IAutoTurretWeapon
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject spawnPoint;

    private Queue<GameObject> missiles;
    private int numMissiles = 5;
    private GameObject missile;

    void Start()
    {
        missiles = new Queue<GameObject>();
        Transform bulletHolder = GameObject.FindGameObjectWithTag("BulletHolder").transform;
        Vector3 spawnPosition = spawnPoint.transform.position;
        Quaternion spawnRotation = spawnPoint.transform.rotation;

        for (int i = 0; i < numMissiles; i++)
        {
            GameObject missile = Instantiate(missilePrefab, spawnPosition, spawnRotation, bulletHolder);
            missile.SetActive(false);
            missiles.Enqueue(missile);
        }
    }

    public void Fire(Collider target)
    {
        missile = missiles.Dequeue();

        if (!missile.activeSelf)
        {
            missile.transform.position = spawnPoint.transform.position;
            missile.transform.rotation = spawnPoint.transform.rotation;

            missile.SetActive(true);
            missile.GetComponent<HomingMissile>().SetTarget(target);
        }

        missiles.Enqueue(missile);
    }

    public void StopFiring()
    {
        //TODO: Missiles should keep firing until told to stop
        return;
    }

    public void Destroyed()
    {
        foreach (GameObject missile in missiles)
        {
            Destroy(missile);
        }
    }
}