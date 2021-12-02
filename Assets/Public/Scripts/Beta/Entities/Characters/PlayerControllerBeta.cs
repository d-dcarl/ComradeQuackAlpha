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

    [Header("Turret Placement")]
    public GameObject placeableTurretPrefab;
    public int maxTurrets;
    protected int numTurrets;
    public float placementDelay;
    protected bool placing;
    protected PlaceableTurretControllerBeta beingPlaced;
    protected float placementTimer;
    public float placementDistance;

    [Header("Miscellanious")]
    public float numResourceTypes;
    protected List<int> inventory;
    public AudioSource audioData;

    [Header("Recruitment")]
    public GameObject recruitCircle;
    private bool recruitActive = false;
    Quaternion deadRotation;
    public float circleExpansionRate = 0.015f;

    [Header("Shooting")]
    public List<GameObject> gunTypes;
    public Transform gunTransform;
    protected GunControllerBeta gunInHand;

    GameObject mesh;

    public override void Start()
    {
        base.Start();

        mesh = transform.Find("Mesh").gameObject;
        recruitCircle.SetActive(false);
        audioData = GetComponent<AudioSource>();

        InitializeStamina();
        InitializeFlying();
        ResetInventory();

        numTurrets = 0;
        placementTimer = placementDelay;
        placing = false;
        beingPlaced = null;

        
        if(gunTypes.Count > 0)
        {
            SwitchWeapons(0);
        }
        else
        {
            Debug.LogError("Error: must start with at least one gun type");
        }
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
        if(recruitActive)
            Recruit();
        if (isGrounded && stamina < maxStamina)
        {
            stamina += staminaRecovery * Time.deltaTime;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = 100f * (stamina / maxStamina);
        }   
    }

    public void SwitchWeapons(int newGun)
    {
        if(gunInHand != null)
        {
            StashWeapon();
        }
        gunInHand = Instantiate(gunTypes[newGun]).GetComponent<GunControllerBeta>();
        gunInHand.transform.parent = gunTransform;
        gunInHand.transform.localPosition = Vector3.zero;
        gunInHand.transform.localRotation = Quaternion.identity;
    }

    public void StashWeapon()
    {
        if(gunInHand != null)
        {
            Destroy(gunInHand.gameObject);
            gunInHand = null;
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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Free cursor and end game on escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Application.Quit();
        }

        //input for player quackling recruitment ring
        if(Input.GetKeyDown(KeyCode.V))
        {
            recruitActive = true;
            audioData.Play();
        }
        if(Input.GetKeyUp(KeyCode.V))
        {
            recruitActive = false;
            EndRecruit();
        }

        PlayerTurning();
        if (alive)
        {
            PlayerMovement();
            TurretPlacement();

            // TODO: Add more gun types, and use scrolling to switch guns
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
        else
        {
            // Make sure dead player doesn't look like it's rotating
            mesh.transform.rotation = deadRotation;
        }
    }

    public void Shoot()
    {
        gunInHand.Shoot();
    }

    void TurretPlacement()
    {
        if (placementTimer > 0f)
        {
            placementTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!placing && numTurrets < maxTurrets && placementTimer <= 0f)
            {
                beingPlaced = Instantiate(placeableTurretPrefab).GetComponent<PlaceableTurretControllerBeta>();
                placing = true;
                numTurrets++;
            }
            else if (placing)
            {
                beingPlaced.PlaceTurret();
                placing = false;
                placementTimer = placementDelay;
            }
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

    private void Recruit()
    {
        float maxSize = 10.0f;
        if(!recruitCircle.activeInHierarchy)
        {
            recruitCircle.SetActive(true);
        }
        recruitCircle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1.0f, this.transform.position.z);
        if (recruitCircle.transform.localScale.x < maxSize)
        {
            recruitCircle.transform.localScale = new Vector3(recruitCircle.transform.localScale.x + circleExpansionRate, recruitCircle.transform.localScale.y, recruitCircle.transform.localScale.z + circleExpansionRate);
        }
        
    }
    private void EndRecruit()
    {
        recruitCircle.transform.localScale = new Vector3(1, 0.1f, 1);
        if (recruitCircle.activeInHierarchy)
        {
            recruitCircle.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    public override void Die()
    {
        alive = false;

        TurnSideways();     
    }

    // Placeholder
    void TurnSideways()
    {
        StashWeapon();
        Vector3 meshRotation = mesh.transform.localEulerAngles;
        mesh.transform.localEulerAngles = new Vector3(meshRotation.x, meshRotation.y, 90f);
        deadRotation = mesh.transform.rotation;
    }

    //This currently will activate from any of the player's trigger colliders. That may need to change if more are added in the future
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RecruitRange")
        {
            //GameObject duckling = other.gameObject.gameObject;
            DucklingControllerBeta duckling_controller = other.GetComponentInParent<DucklingControllerBeta>();
            Debug.Log(duckling_controller);
            if (duckling_controller.GetLeader() == null)
            {
                duckling_controller.SetLeader(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerBeta>());
                duckling_controller.PlayQuack();
            }

        }
    }
}
