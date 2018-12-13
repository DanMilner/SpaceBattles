using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    private RaycastHit hit;
    private int layerMask;
    private int factionID;
    private GameObject objectHit;

    void Awake () {
        layerMask = ~(1 << 2);
        FactionController fc = gameObject.GetComponentInParent<FactionController>();
        if (fc == null)
        {
            factionID = -1;
        }
        else
        {
            factionID = fc.factionID;
        }
    }

    public bool SafeToFire()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 150, layerMask))
        {
            objectHit = hit.transform.gameObject;
            if (objectHit.CompareTag("Ship")) {
                FactionController fc = objectHit.GetComponentInParent<FactionController>();
                return fc != null && fc.factionID != factionID;
            }
        }
        return true;
    }
}
