using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public float missileSpeed = 50.0f;

    public GameObject[] Missiles;
    private int missilesLeft;

    private Rigidbody ShipRigidbody;

    // Use this for initialization
    void Start () {
        missilesLeft = Missiles.Length - 1;
        ShipRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (missilesLeft >= 0) { ShootMissile(); }
        }
    }

    private void ShootMissile()
    {
        GameObject missile = Missiles[missilesLeft--];
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        missile.GetComponent<Rigidbody>().AddForce(missile.transform.forward * missileSpeed);
        missile.GetComponentInChildren<MissileController>().enabled = true;
    }
}
