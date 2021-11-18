using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBeta : CharacterControllerBeta
{
    public float rotationSpeed;
    public float rotationLerp;
    public float targetRotation;
    private float rotation;

    public override void Start()
    {
        base.Start();
        rotation = targetRotation;
    }

    public override void Update()
    {
        base.Update();
        CheckInput();
    }

    protected void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        // Trap cursor when you click the screen
        if(Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Free cursor and end game on escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }

        PlayerTurning();
        PlayerMovement();
    }

    protected void PlayerTurning()
    {
        float mouseVelX = Input.GetAxis("Mouse X");
        targetRotation += mouseVelX * rotationSpeed * Time.deltaTime * 60f;

        rotation = Mathf.Lerp(rotation, targetRotation, rotationLerp);
        transform.localEulerAngles = new Vector3(0f, rotation, 0f);
    }

    protected void PlayerMovement()
    {
        Vector3 direction = Vector3.zero;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        direction += h * transform.right;
        direction += v * transform.forward;

        WalkInDirection(direction);
    }


    void Flap()
    {
        Debug.Log("Flapping");
    }
}
