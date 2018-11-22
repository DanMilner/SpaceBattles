using UnityEngine;
using System.Collections;
 
/*
 * Code taken and modified from: https://forum.unity.com/threads/third-person-camera-rotate.197592/ 
 */

public class CameraController : MonoBehaviour
{
    private Transform CameraTarget;
    private float x = 0.0f;
    private float y = 0.0f;

    private int mouseXSpeedMod = 5;
    private int mouseYSpeedMod = 5;

    public float MaxViewDistance = 15f;
    public float MinViewDistance = 1f;
    public int ZoomRate = 20;
    private int lerpRate = 5;
    private float distance = 3f;
    private float desireDistance;
    private float correctedDistance;
    private float currentDistance;

    public float cameraTargetHeight = 1.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 Angles = transform.eulerAngles;
        x = Angles.x;
        y = Angles.y;
        currentDistance = distance;
        desireDistance = distance;
        correctedDistance = distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {/*0 mouse btn izq, 1 mouse btn der*/
            x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
            y += Input.GetAxis("Mouse Y") * mouseYSpeedMod;
        }
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            float targetRotantionAngle = CameraTarget.eulerAngles.y;
            float cameraRotationAngle = transform.eulerAngles.y;
            x = Mathf.LerpAngle(cameraRotationAngle, targetRotantionAngle, lerpRate * Time.deltaTime);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        desireDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * Mathf.Abs(desireDistance);
        desireDistance = Mathf.Clamp(desireDistance, MinViewDistance, MaxViewDistance);
        correctedDistance = desireDistance;

        Vector3 position = CameraTarget.position - (rotation * Vector3.forward * desireDistance);

        
        RaycastHit collisionHit;
        Vector3 cameraTargetPosition = new Vector3(CameraTarget.position.x, CameraTarget.position.y + cameraTargetHeight, CameraTarget.position.z);

        bool isCorrected = false;
        if (Physics.Linecast(cameraTargetPosition, position, out collisionHit))
        {
            if (!collisionHit.transform.CompareTag("Player")) {
                position = collisionHit.point;
                correctedDistance = Vector3.Distance(cameraTargetPosition, position);
                isCorrected = true;
            }
        }        

        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * ZoomRate) : correctedDistance;

        position = CameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -cameraTargetHeight, 0));

        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetCameraTarget(Transform newCameraTarget, CameraZoomSettings cameraZoomSettings)
    {
        CameraTarget = newCameraTarget;
        MinViewDistance = cameraZoomSettings.MinZoom;
        MaxViewDistance = cameraZoomSettings.MaxZoom;
        ZoomRate = cameraZoomSettings.ZoomRate;
        cameraTargetHeight = cameraZoomSettings.CameraHeight;
    }
}