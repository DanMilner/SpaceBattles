using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] private GameObject[] playerShips;
    [SerializeField] private GameObject shipCamera;
    [SerializeField] private GameObject overviewCamera;

    [SerializeField] private GameObject shipUi;
    [SerializeField] private GameObject overviewUi;

    private CameraController shipCameraController;
    private Camera overViewCamera;
    private GameObject currentPlayerShip;
    private ShipController currentShipController;
    private GameObject currentSelectedShip;
    [SerializeField] bool IsControllingShip;
    private int currentShipNumber = 0;
    private UIHandler uIHandler;

    private int layerMask;

    private bool shipSelected;

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

    void Start () {
        shipCameraController = shipCamera.GetComponent<CameraController>();
        uIHandler = shipUi.GetComponent<UIHandler>();
        overViewCamera = overviewCamera.GetComponent<Camera>();

        if (IsControllingShip)
        {
            PlayerEnterShip(0);
        }
        else
        {
            PlayerEnterOverview();
        }

        layerMask = (1 << 11);
    }

    void Update()
    {
        if (Input.GetButtonDown("Overview"))
        {
            IsControllingShip = !IsControllingShip;
            if (IsControllingShip)
            {
                PlayerEnterShip(currentShipNumber);
            }
            else
            {
                PlayerEnterOverview();
            }
        }

        if (IsControllingShip)
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
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootMouseRaycast(false);
            }else if (Input.GetMouseButtonDown(1))
            {
                if (shipSelected)
                {
                    ShootMouseRaycast(true);
                }
            }
        }
    }

    private void ShootMouseRaycast(bool attack)
    {
        RaycastHit hit;
        Ray ray = overViewCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 2000.0f, layerMask))
        {
            GameObject shipHit = hit.transform.gameObject;
            ShipController shipController = shipHit.GetComponent<ShipController>();
            if (attack)
            {
                if (shipController.GetFactionID() != currentShipController.GetFactionID())
                {
                    DisableOutline(currentShipController.GetAiTarget());
                    currentShipController.SetAiTarget(shipHit);
                    EnableOutline(shipHit, false);
                }
            }
            else
            {
                DisableOutline(currentSelectedShip);
                if(currentShipController != null) { DisableOutline(currentShipController.GetAiTarget()); }
                
                currentSelectedShip = shipHit;
                currentShipController = shipController;

                EnableOutline(shipHit, true);
                EnableOutline(currentShipController.GetAiTarget(), false);

                shipSelected = true;
            }
        }
        else
        {
            DisableOutline(currentSelectedShip);
            if (currentShipController != null) { DisableOutline(currentShipController.GetAiTarget()); }

            currentShipController = null;
            currentSelectedShip = null;

            shipSelected = false;
        }
    }

    private void EnableOutline(GameObject ship, bool friendlyShip)
    {
        if (ship == null) { return; }

        Outline outline = ship.GetComponentInChildren<Outline>();

        if (friendlyShip)
        {
            outline.OutlineColor = Color.blue;
        }
        else
        {
            outline.OutlineColor = Color.red;
        }
        outline.OutlineWidth = 2.0f;
        outline.enabled = true;
    }

    private void DisableOutline(GameObject ship)
    {
        if(ship == null) { return; }

        Outline outline = ship.GetComponentInChildren<Outline>();
        outline.enabled = false;
    }

    private void ChangePlayerShip(int shipNumber)
    {
        if (shipNumber > playerShips.Length) { return; }

        currentShipNumber = shipNumber;

        currentShipController.SetPlayerControlled(false);

        PlayerEnterShip(currentShipNumber);

        uIHandler.SetRotationalStabiliers(currentShipController.AreRotationalStabilisersActive());
        uIHandler.SetMovementStabiliers(currentShipController.AreMovementStabilisersActive());
    }

    private void PlayerEnterShip(int shipNumber)
    {
        shipCamera.SetActive(true);
        shipUi.SetActive(true);
        overviewCamera.SetActive(false);
        overviewUi.SetActive(false);

        currentPlayerShip = playerShips[shipNumber];
        shipCameraController.SetCameraTarget(currentPlayerShip.transform, currentPlayerShip.GetComponent<CameraZoomSettings>());

        currentShipController = currentPlayerShip.GetComponentInChildren<ShipController>();
        currentShipController.SetPlayerControlled(true);
    }

    private void PlayerEnterOverview()
    {
        shipCamera.SetActive(false);
        shipUi.SetActive(false);
        overviewCamera.SetActive(true);
        overviewUi.SetActive(true);

        currentPlayerShip = null;
        
        if(currentShipController != null)
        {
            currentShipController.SetPlayerControlled(false);
            currentShipController = null;
        }
    }
}
