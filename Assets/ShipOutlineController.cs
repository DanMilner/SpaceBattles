using System.Collections.Generic;
using UnityEngine;

public class ShipOutlineController : MonoBehaviour
{
    public int PlayerFactionId;

    private void Start()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            SetOutlineDefault(ship);
        }
    }

    public void SetOutlineDefault(GameObject ship)
    {
        FactionController fc = ship.GetComponentInParent<FactionController>();
        Outline shipOutline = ship.GetComponentInChildren<Outline>();

        if (fc != null && fc.factionID != PlayerFactionId)
        {
            SetEnemyDefault(shipOutline);
        }
        else
        {
            SetFriendlyDefault(shipOutline);
        }
    }
    
    public void SetFriendlyDefault(Outline shipOutline)
    {
        shipOutline.OutlineColor = Color.blue;
        shipOutline.OutlineWidth = 0.5f;
    }

    public void SetEnemyDefault(Outline shipOutline)
    {
        shipOutline.OutlineColor = Color.red;
        shipOutline.OutlineWidth = 0.5f;
    }
    
    public void SetFriendlySelected(Outline shipOutline)
    {
        shipOutline.OutlineColor = Color.blue;
        shipOutline.OutlineWidth = 3.0f;
    }
    
    public void SetEnemySelected(Outline shipOutline)
    {
        shipOutline.OutlineColor = Color.red;
        shipOutline.OutlineWidth = 3.0f;
    }
}