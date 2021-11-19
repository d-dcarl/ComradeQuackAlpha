using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerBeta : CharacterControllerBeta
{
    [Header("Walking and turning")]
    public float rotationSpeed;
    public float rotationLerp;
    protected float targetRotation;
    protected float rotation;
    

    [Header("Jump and Gliding")]
    public float flapSpeed = 30f;
    public float flapDelay = 0.2f;
    protected float flapTimer;
    protected bool isGrounded;
    public float maxFlyHeight = 10f;

    public float sprintSpeed = 12f;
    public float glideMoveSpeed = 10f;
    public float glideFallSpeed = -5f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    protected float stamina;
    public float staminaRecovery = 20f;
    public float staminaUsedPerJump = 20f;
    public float staminaUsedPerGlideSecond = 5f;

    public Slider staminaSlider;

    public override void Start()
    {
        base.Start();
        targetRotation = rotation = transform.eulerAngles.y;
        stamina = maxStamina;
        flapTimer = 0f;

        if(glideFallSpeed > 0f)
        {
            Debug.Log("Make sure your fall speed is negative by convention");
            glideFallSpeed = -1 * Mathf.Abs(glideFallSpeed);
        }

        if(staminaSlider == null)
        {
            Debug.Log("Error: Player has no stamina slider");
        }
    }

    public override void Update()
    {
        base.Update();

        if(flapTimer > 0f)
        {
            flapTimer -= Time.deltaTime;
        }
        CheckInput();
        EnforceMaxHeight();
        if(isGrounded && stamina < maxStamina)
        {
            stamina += staminaRecovery * Time.deltaTime;
        }

        if(staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }
    }

    protected void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && flapTimer <= 0f)
        {
            Flap();
        }

        if(Input.GetButton("Jump") && stamina > 0f)
        {
            Glide();
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

    

    protected void EnforceMaxHeight()
    {
        if (this.transform.position.y > maxFlyHeight)
        {
            transform.position = new Vector3(transform.position.x, maxFlyHeight, transform.position.z);
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }

    protected void PlayerTurning()
    {
        // Don't move camera unless the window has focus
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseVelX = Input.GetAxis("Mouse X");
            targetRotation += mouseVelX * rotationSpeed * Time.deltaTime * 60f;

            rotation = Mathf.Lerp(rotation, targetRotation, rotationLerp);
            transform.eulerAngles = new Vector3(0f, rotation, 0f);
        }
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
        if(stamina >= staminaUsedPerJump)
        {
            Debug.Log("Flapping");
            isGrounded = false;
            rb.velocity = new Vector3(rb.velocity.x, flapSpeed, rb.velocity.y);
            stamina -= staminaUsedPerJump;
            flapTimer = flapDelay;
        }
        else
        {
            Debug.Log("Too tired");
        }
    }

    protected void Glide()
    {
        if (rb.velocity.y < glideFallSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, glideFallSpeed, rb.velocity.z);
        }

        // Don't use up glide power while you still have upward momentum
        if (rb.velocity.y < 0f)
        {
            stamina -= staminaUsedPerGlideSecond * Time.deltaTime;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
