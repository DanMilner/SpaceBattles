using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurretMissiles : MonoBehaviour, IAutoTurretWeapon {
    public GameObject missilePrefab;
    public GameObject spawnPoint;

    private Rigidbody ShipRigidbody;

    void Start()
    {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    public void Fire(GameObject target)
    {
        GameObject missile = Instantiate(missilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        missile.GetComponent<HomingMissile>().SetTarget(target);
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
    }
}
