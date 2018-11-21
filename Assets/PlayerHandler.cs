using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public GameObject CurrentPlayerShip;
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
        cameraController = gameObject.GetComponent<CameraController>();
        cameraController.SetCameraTarget(CurrentPlayerShip.transform, CurrentPlayerShip.GetComponent<CameraZoomSettings>());
        CurrentPlayerShip.GetComponent<FlightController>().enabled = true;
    }
}
