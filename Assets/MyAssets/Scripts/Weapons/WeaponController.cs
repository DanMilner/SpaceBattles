using UnityEngine;

public interface IWeapon
{
    void Fire();
    void SetWeaponTarget(GameObject target);
    string GetName();
}

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject weaponTarget;

    private GameObject mainCamera;
    private UIHandler uIHandler;
    private int currentWeaponNum;
    private IWeapon currentWeapon;
    private int numberOfWeapons;
    private bool playerControlled;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
    }

    private void Start()
    {
        currentWeapon = GetWeapon(currentWeaponNum);
        numberOfWeapons = weapons.Length - 1;
        UpdateUiWithWeaponName();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerControlled)
        {
            return;
        }

        weaponTarget.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 150;
        //TODO. shoot raycast and put target at whatever it hits or put it at the max distance.

        if (!Input.GetKey("left ctrl"))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                currentWeaponNum++;
                ChangeWeapon();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                currentWeaponNum--;
                ChangeWeapon();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (currentWeapon != null)
            {
                currentWeapon.Fire();
            }
        }
    }

    private void ChangeWeapon()
    {
        if (currentWeaponNum > numberOfWeapons)
        {
            currentWeaponNum = 0;
        }
        else if (currentWeaponNum < 0)
        {
            currentWeaponNum = numberOfWeapons;
        }

        currentWeapon = GetWeapon(currentWeaponNum);
        currentWeapon.SetWeaponTarget(weaponTarget);
        UpdateUiWithWeaponName();
    }

    public void SetPlayerControlled(bool pControlled)
    {
        playerControlled = pControlled;
        if (playerControlled)
        {
            UpdateUiWithWeaponName();
            if (currentWeapon != null)
            {
                currentWeapon.SetWeaponTarget(weaponTarget);
            }
        }
    }

    private IWeapon GetWeapon(int weaponNum)
    {
        if (weapons.Length > 0 && weaponNum <= numberOfWeapons)
        {
            return weapons[currentWeaponNum].GetComponent<IWeapon>();
        }
        return null;
    }

    private void UpdateUiWithWeaponName()
    {
        if (uIHandler == null)
        {
            uIHandler = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandler>();
        }

        if (currentWeapon != null)
        {
            uIHandler.SetCurrentWeapon(currentWeapon.GetName());
        }
        else
        {
            uIHandler.SetCurrentWeapon("No Weapons");
        }
    }
}