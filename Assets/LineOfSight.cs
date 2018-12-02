using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {
    private RaycastHit hit;
    private int layerMask;

    void Start () {
        layerMask = ~(1 << 2);
    }

    public bool SafeToFire()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 20, layerMask))
        {           
            return !hit.transform.gameObject.CompareTag("Player");
        }
        return true;
    }
}
