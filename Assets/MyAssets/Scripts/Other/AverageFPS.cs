using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFPS : MonoBehaviour {
    public UIHandler uIHandler;
    float avg = 0F; 
	
	void Update () {
        avg += ((Time.deltaTime / Time.timeScale) - avg) * 0.03f;
        float displayValue = (1F / avg);
        uIHandler.SetAverageFPS(displayValue);
    }
}
