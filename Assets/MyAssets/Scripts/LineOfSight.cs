using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    private RaycastHit hit;
    private int layerMask;
    private FactionID faction;
    void Start () {
        layerMask = ~(1 << 2);
        faction = GetComponentInParent<FactionID>();
    }

    public bool SafeToFire()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 150, layerMask))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.CompareTag("Ship")) {
                return objectHit.GetComponentInParent<FactionID>().factionID != faction.factionID;
            }

            return !objectHit.CompareTag("Player");
        }
        return true;
    }
}
