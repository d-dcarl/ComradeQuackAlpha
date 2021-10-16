using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform target, player;
    float mouseX, mouseY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mouseX += Input.GetAxisRaw("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
