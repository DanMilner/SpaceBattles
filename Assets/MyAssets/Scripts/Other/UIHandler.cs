﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public Text ForwardText;
    public Text HorizontalText;
    public Text VerticalText;

    public Text ForwardTextLocal;
    public Text HorizontalTextLocal;
    public Text VerticalTextLocal;

    public Text PitchText;
    public Text RollText;
    public Text YawText;

    public Text RotationalStabilisers;
    public Text MovementStabilisers;

    public Text CurrentWeapon;

    public Text CurrentHealth;

    public Text FPS;
    public Text AverageFPS;

    public void UpdateUI(Rigidbody playerShip)
    {
        ForwardText.text = playerShip.velocity.z.ToString("F3");
        HorizontalText.text = playerShip.velocity.x.ToString("F3");
        VerticalText.text = playerShip.velocity.y.ToString("F3");

        ForwardTextLocal.text = playerShip.transform.InverseTransformDirection(playerShip.velocity).z.ToString("F3");
        HorizontalTextLocal.text = playerShip.transform.InverseTransformDirection(playerShip.velocity).x.ToString("F3");
        VerticalTextLocal.text = playerShip.transform.InverseTransformDirection(playerShip.velocity).y.ToString("F3");

        //PitchText.text = "Pitch: " + playerShip.angularVelocity.x.ToString("F3") + " " + playerShip.transform.InverseTransformDirection(playerShip.angularVelocity).x.ToString("F3");
        //RollText.text = "Roll: " + playerShip.angularVelocity.z.ToString("F3") + " " + playerShip.transform.InverseTransformDirection(playerShip.angularVelocity).z.ToString("F3");
        //YawText.text = "Yaw: " + playerShip.angularVelocity.y.ToString("F3") + " " + playerShip.transform.InverseTransformDirection(playerShip.angularVelocity).y.ToString("F3");
        PitchText.text = playerShip.angularVelocity.x.ToString("F3");
        RollText.text = playerShip.angularVelocity.z.ToString("F3");
        YawText.text = playerShip.angularVelocity.y.ToString("F3");
    }

    public void SetRotationalStabiliers(bool active)
    {
        if (active)
        {
            RotationalStabilisers.text = "Rotational Stabilisers: Active";
        }
        else
        {
            RotationalStabilisers.text = "Rotational Stabilisers: Inactive";
        }
    }

    public void SetMovementStabiliers(bool active)
    {
        if (active)
        {
            MovementStabilisers.text = "Movement Stabilisers: Active";
        }
        else
        {
            MovementStabilisers.text = "Movement Stabilisers: Inactive";
        }
    }    

    public void SetCurrentWeapon(string CurrentWeaponName)
    {
        CurrentWeapon.text = "Weapon: " + CurrentWeaponName;
    }

    public void SetCurrentHealth(float health)
    {
        CurrentHealth.text = health.ToString();
    }

    public void SetAverageFPS(float val)
    {
        AverageFPS.text = val.ToString();
    }

    public void SetFPS(float val)
    {
        FPS.text = val.ToString();
    }
}
