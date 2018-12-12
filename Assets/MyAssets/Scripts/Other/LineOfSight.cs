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
        factionID = GetComponentInParent<FactionController>().factionID;
    }

    public bool SafeToFire()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 150, layerMask))
        {
            objectHit = hit.transform.gameObject;
            if (objectHit.CompareTag("Ship")) {
                return objectHit.GetComponentInParent<FactionController>().factionID != factionID;
            }
        }
        return true;
    }
}
