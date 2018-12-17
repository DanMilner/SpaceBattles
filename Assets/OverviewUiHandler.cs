using System.Collections.Generic;
using UnityEngine;

public class OverviewUiHandler : MonoBehaviour
{
    [SerializeField] private GameObject shipButtonWindow;
    [SerializeField] private GameObject shipButton;

    public void CreateUi(List<ShipController> shipControllers, PlayerHandler playerHandler)
    {
        int numShips = shipControllers.Count;

        shipButtonWindow.GetComponent<RectTransform>();

        RectTransform rt = shipButtonWindow.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(numShips, 1.2f);

        float offset = numShips / (numShips + 1.0f);
        float startPos = -numShips / 2.0f + offset;
        
        for (int i = 0; i < numShips; i++)
        {
            GameObject button = Instantiate(shipButton, shipButtonWindow.transform);
            button.GetComponent<OverviewButtonHandler>().SetUpButton(shipControllers[i].gameObject, shipControllers[i],playerHandler);
            button.GetComponent<RectTransform>().anchoredPosition  = new Vector2(startPos, -0.02f);
            startPos += offset;
        }
    }
}