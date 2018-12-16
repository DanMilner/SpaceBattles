using UnityEngine;
 
/*
 * Code taken and modified from: https://forum.unity.com/threads/third-person-camera-rotate.197592/ 
 */

public class CameraMovementController : MonoBehaviour
{
    private float x = 0.0f;
    private float y = 0.0f;

    private int mouseXSpeedMod = 5;
    private int mouseYSpeedMod = 5;

    // Use this for initialization
    void Start()
    {
        Vector3 Angles = transform.eulerAngles;
        x = Angles.x;
        y = Angles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
            y += -Input.GetAxis("Mouse Y") * mouseYSpeedMod;
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        transform.rotation = rotation;
    }
}