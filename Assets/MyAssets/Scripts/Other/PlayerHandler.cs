using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    public GameObject[] playerShips;
    public UIHandler uIHandler;

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

    private GameObject currentPlayerShip;
    private CameraController cameraController;
    private ShipController currentShipController;

    // Use this for initialization
    void Start () {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        SetPlayerShip(0);
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

        if (Input.GetButtonDown("RotationalStabilisers"))
        {
            currentShipController.ToggleRotationalStabilisers();
            uIHandler.SetRotationalStabiliers(currentShipController.AreRotationalStabilisersActive());
        }

        if (Input.GetButtonDown("MovementStabilisers"))
        {
            currentShipController.ToggleMovementStabilisers();
            uIHandler.SetMovementStabiliers(currentShipController.AreMovementStabilisersActive());
        }

        uIHandler.UpdateUI(currentShipController.GetRigidbody());
    }

    private void ChangePlayerShip(int shipNumber)
    {
        if (shipNumber > playerShips.Length) { return; }

        currentShipController.SetPlayerControlled(false);

        SetPlayerShip(shipNumber);

        uIHandler.SetRotationalStabiliers(currentShipController.AreRotationalStabilisersActive());
        uIHandler.SetMovementStabiliers(currentShipController.AreMovementStabilisersActive());
    }

    private void SetPlayerShip(int shipNumber)
    {
        if (shipNumber >= playerShips.Length)
        {
            return;
        }

        if (currentPlayerShip != null)
        {
            currentPlayerShip.tag = "Ship";
        }

        currentPlayerShip = playerShips[shipNumber];
        cameraController.SetCameraTarget(currentPlayerShip.transform, currentPlayerShip.GetComponent<CameraZoomSettings>());

        currentShipController = currentPlayerShip.GetComponentInChildren<ShipController>();
        currentShipController.SetPlayerControlled(true);

        currentPlayerShip.tag = "Player";
    }
}
