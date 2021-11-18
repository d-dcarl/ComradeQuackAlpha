using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerBeta : MonoBehaviour
{
    public GameObject player;

    public float cameraDistance;

    public float targetAngle;
    private float cameraAngle;        // measured in degrees above the horizontal

    public float minCamAngle;
    public float maxCamAngle;
    public float cameraSpeed;
    public float cameraLerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.Log("Need to assign player field of camera controller");
        }

        cameraAngle = targetAngle;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        cameraAngle = Mathf.Lerp(cameraAngle, targetAngle, cameraLerpSpeed);

        float degToRads = Mathf.PI / 180f;
        float camBack = Mathf.Cos(cameraAngle * degToRads);
        float camUp = Mathf.Sin(cameraAngle * degToRads);

        Vector3 cameraDirection = (player.transform.forward * -1) * camBack + player.transform.up * camUp;
        Vector3 cameraOffset = cameraDistance * cameraDirection;
        transform.position = player.transform.position + cameraOffset;
        transform.LookAt(player.transform);
    }

    public void CheckInput()
    {
        float mouseVelX = Input.GetAxis("Mouse X");
        float mouseVelY = Input.GetAxis("Mouse Y");

        targetAngle -= mouseVelY * cameraSpeed * Time.deltaTime * 60f;        // Moving mouse up should move camera offset angle down so it points further up
        targetAngle = Mathf.Clamp(targetAngle, minCamAngle, maxCamAngle);
    }
}
