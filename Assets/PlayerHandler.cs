using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    public GameObject[] PlayerShips;

    public GameObject CurrentPlayerShip;
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
        cameraController = gameObject.GetComponent<CameraController>();
        cameraController.SetCameraTarget(CurrentPlayerShip.transform, CurrentPlayerShip.GetComponent<CameraZoomSettings>());
        CurrentPlayerShip.GetComponent<FlightController>().enabled = true;
    }

    void Update()
    {
        if (Input.GetButton("ChangeShip"))
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    ChangePlayerShip(i);
                }
            }
        }
    }

    private void ChangePlayerShip(int shipNumber)
    {
        if (shipNumber > PlayerShips.Length) { return; }

        CurrentPlayerShip.GetComponent<FlightController>().enabled = false;

        CurrentPlayerShip = PlayerShips[shipNumber];
        cameraController.SetCameraTarget(CurrentPlayerShip.transform, CurrentPlayerShip.GetComponent<CameraZoomSettings>());
        CurrentPlayerShip.GetComponent<FlightController>().enabled = true;
    }
}
