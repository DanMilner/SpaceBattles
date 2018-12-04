using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretMissiles : MonoBehaviour, IAutoTurretWeapon {
    public GameObject missilePrefab;
    public GameObject spawnPoint;

    private Rigidbody ShipRigidbody;
    private Queue<GameObject> missiles;
    private int numMissiles = 5;

    void Start()
    {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();


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
        GameObject missile = missiles.Dequeue();

        if (!missile.activeSelf)
        {
            Rigidbody bulletRigidBody = missile.GetComponent<Rigidbody>();

            missile.transform.position = spawnPoint.transform.position;
            missile.transform.rotation = spawnPoint.transform.rotation;

            missile.GetComponent<HomingMissile>().SetTarget(target);
            missile.SetActive(true);

            bulletRigidBody.velocity = ShipRigidbody.velocity;
        }
        missiles.Enqueue(missile);
    }
}
