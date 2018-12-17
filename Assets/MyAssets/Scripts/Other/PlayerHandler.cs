using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] playerShips;
    [SerializeField] private GameObject shipCamera;
    [SerializeField] private GameObject overviewCameraGameObject;
    [SerializeField] private GameObject shipUi;
    [SerializeField] private GameObject overviewUi;
    [SerializeField] private bool isControllingShip;
    [SerializeField] private FactionController factionController;

    private CameraController shipCameraController;
    private Camera overViewCamera;
    private GameObject currentPlayerShip;
    private ShipController currentShipController;
    private GameObject currentSelectedShip;
    private int currentShipNumber;
    private UIHandler uIHandler;
    private OverviewUiHandler overviewUiHandler;

    private int layerMask;

    private bool shipSelected;

    private KeyCode[] keyCodes =
    {
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

    private void Start()
    {
        shipCameraController = shipCamera.GetComponent<CameraController>();
        uIHandler = shipUi.GetComponent<UIHandler>();
        overviewUiHandler = overviewUi.GetComponent<OverviewUiHandler>();
        overViewCamera = overviewCameraGameObject.GetComponent<Camera>();

        if (isControllingShip)
        {
            PlayerEnterShip(0);
        }
        else
        {
            PlayerEnterOverview();
        }

        layerMask = 1 << 11;

        overviewUiHandler.CreateUi(factionController.GetFriendlyShips(), GetComponent<PlayerHandler>());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Overview"))
        {
            isControllingShip = !isControllingShip;
            if (isControllingShip)
            {
                PlayerEnterShip(currentShipNumber);
            }
            else
            {
                PlayerEnterOverview();
            }
        }

        if (isControllingShip)
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
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (shipSelected)
                {
                    ShootMouseRaycast(true);
                }
            }

            /*
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom != 0)
            {
                overviewCameraGameObject.transform.Translate(overviewCameraGameObject.transform.forward * Time.deltaTime * zoom * 500, Space.World);
            }
            */

            float horiz = Input.GetAxis("MoveCameraHorizontal");
            if (horiz != 0)
            {
                overviewCameraGameObject.transform.Translate(
                    overviewCameraGameObject.transform.right * Time.deltaTime * horiz * 300, Space.World);
            }

            float vert = Input.GetAxis("MoveCameraVertical");
            if (vert != 0)
            {
                overviewCameraGameObject.transform.Translate(
                    overviewCameraGameObject.transform.forward * Time.deltaTime * vert * 300, Space.World);
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
                SelectShip(shipHit, shipController);
            }
        }
        else
        {
            DisableOutline(currentSelectedShip);
            if (currentShipController != null)
            {
                DisableOutline(currentShipController.GetAiTarget());
            }

            currentShipController = null;
            currentSelectedShip = null;

            shipSelected = false;
        }
    }

    public void SelectShip(GameObject shipGameObject, ShipController shipController)
    {
        DisableOutline(currentSelectedShip);
        if (currentShipController != null)
        {
            DisableOutline(currentShipController.GetAiTarget());
        }

        currentSelectedShip = shipGameObject;
        currentShipController = shipController;

        EnableOutline(shipGameObject, true);
        EnableOutline(currentShipController.GetAiTarget(), false);

        shipSelected = true;
    }

    private static void EnableOutline(GameObject ship, bool friendlyShip)
    {
        if (ship == null)
        {
            return;
        }

        Outline outline = ship.GetComponentInChildren<Outline>();

        outline.OutlineColor = friendlyShip ? Color.blue : Color.red;
        outline.OutlineWidth = 2.0f;
        outline.enabled = true;
    }

    private static void DisableOutline(GameObject ship)
    {
        if (ship == null)
        {
            return;
        }

        Outline outline = ship.GetComponentInChildren<Outline>();
        outline.enabled = false;
    }

    private void ChangePlayerShip(int shipNumber)
    {
        if (shipNumber > playerShips.Length)
        {
            return;
        }

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
        overviewCameraGameObject.SetActive(false);
        overviewUi.SetActive(false);

        currentPlayerShip = playerShips[shipNumber];
        shipCameraController.SetCameraTarget(currentPlayerShip.transform,
            currentPlayerShip.GetComponent<CameraZoomSettings>());

        currentShipController = currentPlayerShip.GetComponentInChildren<ShipController>();
        currentShipController.SetPlayerControlled(true);
    }

    private void PlayerEnterOverview()
    {
        shipCamera.SetActive(false);
        shipUi.SetActive(false);
        overviewCameraGameObject.SetActive(true);
        overviewUi.SetActive(true);

        currentPlayerShip = null;

        if (currentShipController == null) return;
        currentShipController.SetPlayerControlled(false);
        currentShipController = null;
    }
}