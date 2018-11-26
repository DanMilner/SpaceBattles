using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : MonoBehaviour, IWeapon
{
    public GameObject missilePrefab;
    public GameObject spawnPoint;
    public float fireRate = 2.0f;
    private float cooldown;
    private Rigidbody ShipRigidbody;
    private GameObject target;

    public void Start()
    {
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    public void Update()
    {
        cooldown -= Time.deltaTime;
    }

    public void Fire()
    {
        if (cooldown < 0)
        {
            ShootMissile();
            cooldown = fireRate;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void ShootMissile()
    {
        GameObject missile = Instantiate(missilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        missile.GetComponent<HomingMissile>().SetTarget(target);
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
    }

    public string GetName()
    {
        return "Gun";
    }
}
