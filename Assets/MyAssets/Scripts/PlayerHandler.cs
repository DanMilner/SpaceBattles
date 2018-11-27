using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    public GameObject[] PlayerShips;
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

    private GameObject CurrentPlayerShip;
    private CameraController cameraController;
    private FlightController CurrentFlightController;
    private WeaponController currentWeaponController;
    private Rigidbody CurrentShipRigidbody;

    // Use this for initialization
    void Start () {
        cameraController = gameObject.GetComponent<CameraController>();

        SetPlayerShip(0);

        uIHandler.SetRotationalStabiliers(CurrentFlightController.AreRotationalStabilisersActive());
        uIHandler.SetMovementStabiliers(CurrentFlightController.AreMovementStabilisersActive());
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
            CurrentFlightController.ToggleRotationalStabilisers();
            uIHandler.SetRotationalStabiliers(CurrentFlightController.AreRotationalStabilisersActive());
        }

        if (Input.GetButtonDown("MovementStabilisers"))
        {
            CurrentFlightController.ToggleMovementStabilisers();
            uIHandler.SetMovementStabiliers(CurrentFlightController.AreMovementStabilisersActive());
        }

        uIHandler.UpdateUI(CurrentShipRigidbody);
    }

    private void ChangePlayerShip(int shipNumber)
    {
        if (shipNumber > PlayerShips.Length) { return; }

        CurrentFlightController.SetPlayerControlled(false);
        currentWeaponController.SetPlayerControlled(false);

        SetPlayerShip(shipNumber);

        uIHandler.SetRotationalStabiliers(CurrentFlightController.AreRotationalStabilisersActive());
        uIHandler.SetMovementStabiliers(CurrentFlightController.AreMovementStabilisersActive());
    }

    private void SetPlayerShip(int shipNumber)
    {
        if(CurrentPlayerShip != null)
        {
            CurrentPlayerShip.tag = "Untagged";
        }
        CurrentPlayerShip = PlayerShips[shipNumber];
        cameraController.SetCameraTarget(CurrentPlayerShip.transform, CurrentPlayerShip.GetComponent<CameraZoomSettings>());
        CurrentFlightController = CurrentPlayerShip.GetComponent<FlightController>();
        currentWeaponController = CurrentPlayerShip.GetComponent<WeaponController>();
        CurrentFlightController.SetPlayerControlled(true);
        currentWeaponController.SetPlayerControlled(true);
        CurrentShipRigidbody = CurrentPlayerShip.GetComponent<Rigidbody>();
        CurrentPlayerShip.tag = "Player";
    }
}
