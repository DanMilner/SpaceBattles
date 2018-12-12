using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour, IWeapon
{
    public GameObject[] Missiles;

    private int missilesLeft;
    private Rigidbody ShipRigidbody;

    private float MissileCooldown = 0.6f;
    private float MissileCooldownTimer;

    void Awake () {
        missilesLeft = Missiles.Length - 1;
        ShipRigidbody = gameObject.GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        MissileCooldownTimer -= Time.deltaTime;
    }

    public void Fire()
    {
        if (MissileCooldownTimer < 0 && missilesLeft >= 0) {
            ShootMissile();
            MissileCooldownTimer = MissileCooldown;
        }
    }

    private void ShootMissile()
    {
        GameObject missile = Missiles[missilesLeft--];
        missile.transform.parent = null;
        missile.GetComponent<Rigidbody>().velocity = ShipRigidbody.velocity;
        missile.GetComponentInChildren<IndividualMissileController>().enabled = true;
    }
    public string GetName()
    {
        return "Missiles";
    }

    public void SetWeaponTarget(GameObject target)
    {
        //TODO: implement weapon target. Gun should face where the player is looking.
        return;
    }
}
