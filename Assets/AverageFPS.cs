using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFPS : MonoBehaviour {
    public UIHandler uIHandler;
    float avg = 0F; //declare this variable outside Update
	
	// Update is called once per frame
	void Update () {
        avg += ((Time.deltaTime / Time.timeScale) - avg) * 0.03f; //run this every frame
        float displayValue = (1F / avg); //display this value
        uIHandler.SetAverageFPS(displayValue);
    }
}
