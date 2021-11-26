using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerBeta : CharacterControllerBeta
{
    [Header("Movement")]
    public float flapSpeed = 30f;
    public float flapDelay = 0.2f;
    protected float flapTimer;
    protected bool isGrounded;
    public float maxFlyHeight = 10f;

    public float sprintSpeed = 12f;
    public float glideMoveSpeed = 10f;
    public float glideFallSpeed = -5f;
    public float rotationSpeed;

    [Header("Stamina")]
    public float maxStamina = 200f;
    protected float stamina;
    public float staminaRecovery = 20f;
    public float staminaUsedPerJump = 20f;
    public float staminaUsedPerGlideSecond = 5f;

    public float foodStaminaRegen;      // Just an idea

    public Slider staminaSlider;

    protected bool alive;
    GameObject mesh;
    Quaternion deadRotation;

    public float numResourceTypes;
    protected List<int> inventory;

    public override void Start()
    {
        base.Start();

        alive = true;
        mesh = transform.Find("Mesh").gameObject;

        InitializeStamina();
        InitializeFlying();
        ResetInventory();
    }

    public override void Update()
    {
        base.Update();

        if (flapTimer > 0f)
        {
            flapTimer -= Time.deltaTime;
        }
        CheckInput();
        EnforceMaxHeight();
        if (isGrounded && stamina < maxStamina)
        {
            stamina += staminaRecovery * Time.deltaTime;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = 100f * (stamina / maxStamina);
        }
    }

    protected void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && flapTimer <= 0f && alive)
        {
            Flap();
        }

        if (Input.GetButton("Jump") && stamina > 0f && alive)
        {
            Glide();
        }

        // Trap cursor when you click the screen
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Free cursor and end game on escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }

        PlayerTurning();
        if (alive)
        {
            PlayerMovement();
        }
        else
        {
            // Make sure dead player doesn't look like it's rotating
            mesh.transform.rotation = deadRotation;
        }
    }


    void InitializeStamina()
    {
        if (staminaSlider == null)
        {
            Debug.Log("Error: Player has no stamina slider");
        }
        stamina = maxStamina;
    }

    void InitializeFlying()
    {
        if (glideFallSpeed > 0f)
        {
            Debug.Log("Make sure your fall speed is negative by convention");
            glideFallSpeed = -1 * Mathf.Abs(glideFallSpeed);
        }

        flapTimer = 0f;
    }

    void ResetInventory()
    {
        // For now, only one type. 0 = fly collectibles. Eventually maybe an enum would help
        inventory = new List<int>();
        for (int i = 0; i < numResourceTypes; i++)
        {
            inventory.Add(0);
        }
    }

    public void CollectResource(int resourceType, int amount)
    {
        if (resourceType >= 0 && resourceType < inventory.Count)
        {
            inventory[resourceType] += amount;
            Debug.Log("Collected " + amount + " of resource type " + resourceType + ", putting us at " + inventory[resourceType]);
        } else
        {
            Debug.LogError("Error: Must be a valid resource type");
        }

        // Make food refill stamina maybe?
        if(resourceType == 0)
        {
            stamina += (float)amount * foodStaminaRegen;
            Debug.Log("Yummy!");
        }

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
            if(Mathf.Abs(mouseVelX) > 0.1f)     // Make a rotation deadzome to avoid unintended rotation
            {
                float rotationDelta = mouseVelX * rotationSpeed * Time.deltaTime * 60f;
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + rotationDelta, 0f);
            }
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
            isGrounded = false;
            rb.velocity = new Vector3(rb.velocity.x, flapSpeed, rb.velocity.y);
            stamina -= staminaUsedPerJump;
            flapTimer = flapDelay;
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

    public override void Die()
    {
        Debug.Log("Player is dead");
        alive = false;
        Vector3 meshRotation = mesh.transform.localEulerAngles;
        // Turn sideways
        mesh.transform.localEulerAngles = new Vector3(meshRotation.x, meshRotation.y, 90f);
        deadRotation = mesh.transform.rotation;
    }
}
