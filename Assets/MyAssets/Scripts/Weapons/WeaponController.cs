using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Fire();
    void SetWeaponTarget(GameObject target);
    string GetName();
}

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] Weapons;
    [SerializeField] private GameObject weaponTarget;

    private GameObject mainCamera;
    private UIHandler uIHandler;
    private int CurrentWeaponNum = 0;
    private IWeapon CurrentWeapon;
    private int NumberOfWeapons = 0;
    private bool playerControlled = false;

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
    }

    void Start()
    {
        CurrentWeapon = GetWeapon(CurrentWeaponNum);
        NumberOfWeapons = Weapons.Length-1;
        UpdateUIWithWeaponName();
    }

    // Update is called once per frame
    void Update () {
        if (!playerControlled)
        {
            return;
        }

        weaponTarget.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 50;
        //TODO. shoot raycast and put target at whatever it hits or put it at the max distance.

        if (!Input.GetKey("left ctrl"))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                CurrentWeaponNum++;
                ChangeWeapon();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                CurrentWeaponNum--;
                ChangeWeapon();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if(CurrentWeapon != null)
            {
                CurrentWeapon.Fire();
            }
        }
    }

    private void ChangeWeapon()
    {
        if (CurrentWeaponNum > NumberOfWeapons)
        {
            CurrentWeaponNum = 0;
        }
        else if (CurrentWeaponNum < 0)
        {
            CurrentWeaponNum = NumberOfWeapons;
        }
        CurrentWeapon = GetWeapon(CurrentWeaponNum);
        CurrentWeapon.SetWeaponTarget(weaponTarget);
        UpdateUIWithWeaponName();
    }

    public void SetPlayerControlled(bool pControlled)
    {
        playerControlled = pControlled;
        if (playerControlled)
        {
            UpdateUIWithWeaponName();
            if(CurrentWeapon != null)
            {
                CurrentWeapon.SetWeaponTarget(weaponTarget);
            }
        }
    }

    private IWeapon GetWeapon(int weaponNum)
    {
        if (Weapons.Length > 0 && weaponNum <= NumberOfWeapons)
        {
            return Weapons[CurrentWeaponNum].GetComponent<IWeapon>();
        }
        else
        {
            return null;
        }
    }

    private void UpdateUIWithWeaponName()
    {
        if(uIHandler == null)
        {
            uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
        }
        if (CurrentWeapon != null)
        {
            uIHandler.SetCurrentWeapon(CurrentWeapon.GetName());
        }
        else
        {
            uIHandler.SetCurrentWeapon("No Weapons");
        }
    }
}
