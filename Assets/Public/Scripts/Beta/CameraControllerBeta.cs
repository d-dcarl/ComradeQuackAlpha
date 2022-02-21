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

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.Log("Need to assign player field of camera controller");
        }

        cameraAngle = targetAngle;
        degToRads = Mathf.PI / 180f;

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Vector3 cameraDirection, cameraOffset;
        float camBack, camUp;
        if (Input.GetMouseButton(1) || Input.GetAxis("Zoom") > 0f)        // zoomed in
        {
            //--------------------------ZOOM CAM BEHAVIOR-----------------------------------------------------------
            Camera mycam = GetComponent<Camera>();

            float sensitivity = 0.01f; // changed from 0.05 to 0.01
            Vector3 vp = mycam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane));
            vp.x -= 0.5f;
            vp.y -= 0.5f;
            vp.x *= sensitivity;
            vp.y *= sensitivity;
            vp.x += 0.5f;
            vp.y += 0.5f;
            Vector3 sp = mycam.ViewportToScreenPoint(vp);

            Vector3 v = mycam.ScreenToWorldPoint(sp);
            transform.LookAt(v, Vector3.up);

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
