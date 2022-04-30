using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float sensitivityFactor;

    private float rotationSpeed = 2.0f;
    private float pitch;
    private float yaw;

    private bool canZoom = true;

    //pause game stuff
    private PlayerControllerBeta playerController;
    private Canvas pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            Debug.Log("Need to assign player field of camera controller");
        }

        cameraAngle = targetAngle;
        degToRads = Mathf.PI / 180f;

        PlaceZoomCam();

        //pausegamestuff
        playerController = player.GetComponent<PlayerControllerBeta>();
        pauseCanvas = gameObject.GetComponentInChildren<Canvas>();
        pauseCanvas.enabled = false;

        // Get sensitivity
        sensitivityFactor = PlayerPrefs.GetFloat("sensitivity");
        //Debug.Log("Here it is: " + sensitivityFactor);
        
    }

    // Update is called once per frame
    void Update()
    {
        //pause game stuff
        if(playerController.paused)
        {
            //if paused stop doing things
            pauseCanvas.enabled = true;
            return;
        }
        else
        {
            if(pauseCanvas.enabled == true)
            {
                pauseCanvas.enabled = false;
            }
        }

        CheckInput();
        Vector3 cameraDirection, cameraOffset;
        float camBack, camUp;
        if ((Input.GetMouseButton(1) || Input.GetAxis("Zoom") != 0f) && canZoom)    // zoomed in
        {
            pitch += rotationSpeed * Input.GetAxis("Mouse Y") * sensitivityFactor;
            yaw += rotationSpeed * Input.GetAxis("Mouse X") * sensitivityFactor;

            // Clamp pitch:
            pitch = Mathf.Clamp(pitch, -30f, 30f);

            // Wrap yaw:
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }

            //transform.eulerAngles = new Vector3(-pitch, yaw, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(-pitch, yaw, 0f)), 0.1f);

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
    }

    //PlayerPrefs.setflow. set mouse sensitivity
        // Player


    public void resumeGame()
    {
        playerController.pauseGame(true);
    }

    public void goToMainMenu()
    {
        //Change later to main menu scene
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
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

    public void SetCanZoom(bool zoom)
    {
        Debug.Log(this.canZoom);
        this.canZoom = zoom;
        Debug.Log(this.canZoom);
    }

    private void PlaceZoomCam()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene.Equals("Tutorial2"))
        {
            zoomCam.position = player.transform.position + new Vector3(-3, 1, -1); // back, up, right
        }
        else if (scene.Equals("Level 1"))
        {
            zoomCam.position = player.transform.position + new Vector3(-2.5f, 1, 3); // right, up, back
        }
        else if (scene.Equals("WhiteBox1Beta"))
        {
            zoomCam.position = zoomCam.position + new Vector3(2, 0, 1); // 
        }
    }
}
