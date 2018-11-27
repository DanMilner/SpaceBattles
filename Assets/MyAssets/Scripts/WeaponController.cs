using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Fire();
    string GetName();
}

public class WeaponController : MonoBehaviour
{
    public GameObject[] Weapons;
    public UIHandler uIHandler;

    private int CurrentWeaponNum = 0;
    private IWeapon CurrentWeapon;
    private int NumberOfWeapons = 0;

    private bool playerControlled = false;

    void Start()
    {
        CurrentWeapon = Weapons[CurrentWeaponNum].GetComponent<IWeapon>();
        NumberOfWeapons = Weapons.Length-1;
        uIHandler.SetCurrentWeapon(CurrentWeapon.GetName());
    }

    // Update is called once per frame
    void Update () {
        if (!playerControlled)
        {
            return;
        }

        if (!Input.GetKey("left ctrl"))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                CurrentWeaponNum++;
                if (CurrentWeaponNum > NumberOfWeapons)
                {
                    CurrentWeaponNum = 0;
                }
                CurrentWeapon = Weapons[CurrentWeaponNum].GetComponent<IWeapon>();
                uIHandler.SetCurrentWeapon(CurrentWeapon.GetName());
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                CurrentWeaponNum--;
                if (CurrentWeaponNum < 0)
                {
                    CurrentWeaponNum = NumberOfWeapons;
                }
                CurrentWeapon = Weapons[CurrentWeaponNum].GetComponent<IWeapon>();
                uIHandler.SetCurrentWeapon(CurrentWeapon.GetName());
            }
        }

        if (Input.GetMouseButton(0))
        {
            CurrentWeapon.Fire();
        }
    }

    public void SetPlayerControlled(bool pControlled)
    {
        playerControlled = pControlled;
        if (playerControlled)
        {
            uIHandler.SetCurrentWeapon(CurrentWeapon.GetName());
        }
    }
}
