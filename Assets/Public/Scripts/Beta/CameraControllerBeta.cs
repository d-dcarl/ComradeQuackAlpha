using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerBeta : MonoBehaviour
{
    public GameObject player;
    float degToRads;

    public float cameraDistance;

    public float targetAngle;
    private float cameraAngle;        // measured in degrees above the horizontal

    public float minCamAngle;
    public float maxCamAngle;
    public float cameraSpeed;
    public float cameraLerpSpeed;

    public Transform zoomCam;
    public float zoomSpeed = .5f;
    public float zoomXRotationLimit = 30f;  // 45

    private float zoomHighBound;
    private float zoomLowBound;
    private float zoomMidpoint = 180f;

    private float rotationSpeed = 2.0f;
    private float pitch;
    private float yaw;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.Log("Need to assign player field of camera controller");
        }

        cameraAngle = targetAngle;
        degToRads = Mathf.PI / 180f;

        // set zoom rotation boundaries
        zoomHighBound = 360f - zoomXRotationLimit;
        zoomLowBound = 0f + zoomXRotationLimit;

        // adjust zoomCam position
        //zoomCam.position = zoomCam.position + new Vector3(1, 0, 1);
        //zoomCam.position = zoomCam.position + new Vector3(2, 0, 2);
        zoomCam.position = zoomCam.position + new Vector3(2, 0, 1);
        //zoomCam.position = GetComponent<Camera>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckInput();
        Vector3 cameraDirection, cameraOffset;
        float camBack, camUp;
        if (Input.GetMouseButton(1) || Input.GetAxis("Zoom") != 0f)    // zoomed in
        {
            pitch += rotationSpeed * Input.GetAxis("Mouse Y");
            yaw += rotationSpeed * Input.GetAxis("Mouse X");

            // Clamp pitch:
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            // Wrap yaw:
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }

            transform.eulerAngles = new Vector3(-pitch, yaw, 0f);

            // Apply position and rotation to camera and player
            transform.position = Vector3.Slerp(transform.position, zoomCam.position, zoomSpeed * Time.time);
            player.transform.rotation = transform.rotation;
            //------------------------------------------------------------------------------------------------------
        }
        else
        {
            cameraAngle = Mathf.Lerp(cameraAngle, targetAngle, cameraLerpSpeed);

            camBack = Mathf.Cos(cameraAngle * degToRads);
            camUp = Mathf.Sin(cameraAngle * degToRads);

            cameraDirection = (player.transform.forward * -1) * camBack + player.transform.up * camUp;
            cameraOffset = cameraDistance * cameraDirection;

            transform.position = player.transform.position + cameraOffset;
            transform.LookAt(player.transform);

            // set zoom angle
            pitch = player.transform.eulerAngles.x;
            yaw = player.transform.eulerAngles.y;
        }

        //Debug.Log("Camera Angle: " + cameraAngle);
        
    }

    public void CheckInput()
    {
        // Don't move camera unless window has focus
        if(Cursor.lockState == CursorLockMode.Locked || Cursor.lockState == CursorLockMode.Confined)
        {
            float mouseVelX = Input.GetAxis("Mouse X");
            float mouseVelY = Input.GetAxis("Mouse Y");

            targetAngle -= mouseVelY * cameraSpeed * Time.deltaTime * 60f;        // Moving mouse up should move camera offset angle down so it points further up
            targetAngle = Mathf.Clamp(targetAngle, minCamAngle, maxCamAngle);
        }
    }
}
