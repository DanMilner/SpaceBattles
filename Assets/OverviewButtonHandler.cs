using UnityEngine;

public class OverviewButtonHandler : MonoBehaviour
{
    private GameObject ship;
    private ShipController shipController;
    private PlayerHandler playerHandler;

    public void SetUpButton(GameObject ship, ShipController shipController, PlayerHandler playerHandler)
    {
        this.ship = ship;
        this.shipController = shipController;
        this.playerHandler = playerHandler;
    }

    public void SelectShip()
    {
        Debug.Log("Selecting Ship");
        playerHandler.SelectShip(ship, shipController);
    }
}