using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionController : MonoBehaviour {
    public int factionID;
	// Use this for initialization
	void Start () {
        FactionID[] factionIDs = gameObject.GetComponentsInChildren<FactionID>();

        for(int i = 0; i < factionIDs.Length; i++)
        {
            factionIDs[i].factionID = factionID;
        }
	}
}
