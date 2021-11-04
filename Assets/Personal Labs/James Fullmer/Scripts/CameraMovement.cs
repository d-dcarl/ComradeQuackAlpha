using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform target, player, cameraZoom, cameraOut;
    public float zoomSpeed = .5f;
    float mouseX, mouseY;
    bool isZoomedIn;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isZoomedIn = false;
    }

    private void Update()
    {
        
        if (Input.GetButton("Fire2"))
        {
            isZoomedIn = true;
            transform.position = Vector3.Slerp(transform.position, cameraZoom.position, zoomSpeed * Time.time);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraZoom.rotation, zoomSpeed * Time.time);
        }
        else //if (Input.GetButtonUp("Fire2"))
        {
            isZoomedIn = false;
            transform.position = Vector3.Slerp(transform.position, cameraOut.position, zoomSpeed * Time.time);
            //transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(), zoomSpeed * Time.time);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!GameManager.Instance.cameraTransitioning)
        {
            mouseX += Input.GetAxisRaw("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxisRaw("Mouse Y") * rotationSpeed;
            if (isZoomedIn)
                mouseY = Mathf.Clamp(mouseY, -75, 60);
            else
                mouseY = Mathf.Clamp(mouseY, -35, 60);

            if (!isZoomedIn)
                transform.LookAt(target);

            target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            if (isZoomedIn)
            {
                player.rotation = Quaternion.Euler(0, mouseX, 0);
            }
        }
    }
}
