using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

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
    public List<GameObject> placeableTurretPrefabs;
    public int maxTurrets;
    protected int numTurrets;
    public float placementDelay;
    protected bool placing;
    protected PlaceableTurretControllerBeta beingPlaced;
    protected float placementTimer;
    public float placementDistance;
    private int turretPrefabIndex;
    private PlaceableTurretControllerBeta lookedTurret;
    [SerializeField] private GameObject ducklingPrefab;

    [Header("Barricade Placement")]
    public GameObject barricadePrefab;
    public int maxBarricades;
    protected int numBarricades;
    protected BarricadeControllerBeta barricadeBeingPlaced;
    protected bool placingBarricade;
    public List<GameObject> currBarricades;

    [Header("Nest Placement")]
    public GameObject nestPrefab;
    public int maxNests;
    protected int numNests;
    protected PlacableNestControllerBeta nestBeingPlaced;
    protected bool placingNest;
    public List<GameObject> currNests;


    [Header("Miscellanious")]
    public float numResourceTypes;
    protected List<int> inventory;
    public AudioSource audioData;

    [Header("Recruitment")]
    public GameObject recruitCircle;
    private bool recruitActive = false;
    Quaternion deadRotation;
    public float circleExpansionRate = 0.015f;
    public List<DucklingControllerBeta> ducklingsList;
    public int maxDucklings = 12;
    private bool ducklingToTurret = false;
    private bool removeDuckling = false;


    [Header("Shooting")]
    public List<GameObject> gunTypes;
    public Transform gunTransform;
    protected GunControllerBeta gunInHand;

    [Header("Animation")]
    private Animator animator;
    private bool anim_isLanding = false;
    private float anim_shoot_timer = 0f;

    public GameObject deathOverlay;

    //sound instance needed for playing audio
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.EventInstance glideInstance;
    private FMOD.Studio.EventInstance walkInstance;
    private bool glide_playing;
    private float shoot_sound_timer;
    private bool recruitingSound = false;
    private bool walkPlaying = false;

    //pause the game
    private bool paused = false;

    GameObject mesh;

    public override void Start()
    {
        base.Start();

        mesh = transform.Find("Mesh").gameObject;
        recruitCircle.SetActive(false);

        InitializeStamina();
        InitializeFlying();
        ResetInventory();

        numTurrets = 0;
        placementTimer = placementDelay;
        placing = false;
        beingPlaced = null;

        // Barricade Placing Initialization
        numBarricades = 0;
        placingBarricade = false;
        barricadeBeingPlaced = null;
        currBarricades = new List<GameObject>();

        animator = GetComponentInChildren<Animator>();
        animator.Play("Duck Idle (handgun)");


        if (gunTypes.Count > 0)
        {
            SwitchWeapons(0);
        }
        else
        {
            Debug.LogError("Error: must start with at least one gun type");
        }

        lookedTurret = null;
    }

    public override void Update()
    {
        base.Update();
        TurretLook();
        //NestLook();

        if (flapTimer > 0f)
        {
            flapTimer -= Time.deltaTime;
        }
        if(anim_shoot_timer > 0f)
        {
            anim_shoot_timer -= Time.deltaTime;
        }
        if(shoot_sound_timer > 0f)
        {
            shoot_sound_timer -= Time.deltaTime;
        }
        CheckInput();
        EnforceMaxHeight();
        if (recruitActive)
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

    public int GetTurretIndex()
    {
        return turretPrefabIndex;
    }

    public void SwitchWeapons(int newGun)
    {
        if (gunInHand != null)
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
        if (gunInHand != null)
        {
            Destroy(gunInHand.gameObject);
            gunInHand = null;
        }
    }

    protected void CheckInput()
    {

        // Free cursor and pause game on escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 1;
            }
            else
            {
                paused = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            }
            //Application.Quit();
        }
        //if the game is paused don't check for other inputs
        if(paused)
        {
            return;
        }

        if (Input.GetButtonDown("Jump") && flapTimer <= 0f && alive)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/characters/player/jump", GetComponent<Transform>().position);
            Flap();
        }

        if (Input.GetButton("Jump") && stamina > 0f && alive)
        {
            if(!glide_playing)
            {
                glideInstance = FMODUnity.RuntimeManager.CreateInstance("event:/characters/player/gliding");
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(glideInstance, GetComponent<Transform>(), GetComponent<Rigidbody>());
                glideInstance.start();
                glide_playing = true;
            }
            Glide();
        }

        if(Input.GetButtonUp("Jump") || stamina <= 0f ||  !alive)
        {
            glideInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            glide_playing = false;
        }

        /* Trap cursor when you click the screen
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        } */
        Cursor.lockState = CursorLockMode.Locked;           // changed to work with shooting - SJ




        //input for player quackling recruitment ring
        if (Input.GetKeyDown(KeyCode.V) || Input.GetButtonDown("Recruit"))
        {
            recruitActive = true;
        }
        if (Input.GetKeyUp(KeyCode.V) || Input.GetButtonUp("Recruit"))
        {
            recruitActive = false;
            EndRecruit();
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("DucklingTurret"))
        {
            ducklingToTurret = true;
        }

        if (Input.GetKeyUp(KeyCode.C) || Input.GetButtonUp("DucklingTurret"))
        {
            ducklingToTurret = false;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            removeDuckling = true;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            removeDuckling = false;
        }

        PlayerTurning();
        if (alive)
        {
            PlayerMovement();
            TurretPlacement();
            BarricadePlacement();
            //NestPlacement();

            // TODO: Add more gun types, and use scrolling to switch guns
            if (Input.GetMouseButton(0) || Input.GetAxis("Shoot") > 0f)
            {
                if(shoot_sound_timer <= 0)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/weapons/pistol/shoot", GetComponent<Transform>().position);
                    shoot_sound_timer = gunInHand.shootDelay;
                }
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
        if(anim_shoot_timer <= 0 )
        {
            animator.Play("Duck gun shooting (handgun)");
            animator.SetTrigger("Shooting");
            anim_shoot_timer = gunInHand.shootDelay;
        }
        gunInHand.Shoot();
    }

    void TurretPlacement()
    {
        if (placementTimer > 0f)
        {
            placementTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("PlaceTurret"))
        {
            if (!placing && !placingBarricade && !placingNest && numTurrets < maxTurrets && placementTimer <= 0f) // added check for placingBarricade - SJ
            {
                beingPlaced = Instantiate(placeableTurretPrefabs[turretPrefabIndex]).GetComponent<PlaceableTurretControllerBeta>();
                placing = true;
                numTurrets++;
            }
            else if (placing && beingPlaced.IsValidPlacementLocation)
            {
                beingPlaced.PlaceTurret();
                placing = false;
                placementTimer = placementDelay;
            }
        }

        if (placing)
        {
            //cancel the turret placement
            if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Cancel"))
            {
                placing = false;
                Destroy(beingPlaced.gameObject);
            }

            if (Input.mouseScrollDelta.y > 0 || Input.GetButtonDown("NextBumper"))
            {
                turretPrefabIndex--;
                if (turretPrefabIndex < 0)
                {
                    turretPrefabIndex = placeableTurretPrefabs.Count - 1;
                }
                
                Destroy(beingPlaced.gameObject);
                beingPlaced = Instantiate(placeableTurretPrefabs[turretPrefabIndex]).GetComponent<PlaceableTurretControllerBeta>();
            } 
            else if (Input.mouseScrollDelta.y < 0 || Input.GetButtonDown("PrevBumper"))
            {
                turretPrefabIndex++;
                if (turretPrefabIndex >= placeableTurretPrefabs.Count)
                {
                    turretPrefabIndex = 0;
                }
                
                Destroy(beingPlaced.gameObject);
                beingPlaced = Instantiate(placeableTurretPrefabs[turretPrefabIndex]).GetComponent<PlaceableTurretControllerBeta>();
            }
        }
    }

    void BarricadePlacement()
    {
        // Check for destroyed barricades and remove them
        List<int> remove = new List<int>();
        for(int i = 0; i < currBarricades.Count; i++)
        {
            if (currBarricades[i] == null && !ReferenceEquals(currBarricades[i], null))
            {
                remove.Add(i);
                numBarricades--;
            }
        }
        for (int i = 0; i < remove.Count; i++)
            currBarricades.RemoveAt(remove[i]);

        // Place barricades
        if (Input.GetKeyDown(KeyCode.R)) // || Input.GetButtonDown("PlaceBarricade")) -- not yet implemented
        {
            if (!placingBarricade && !placing && !placingNest && numBarricades < maxBarricades)
            {
                barricadeBeingPlaced = Instantiate(barricadePrefab).GetComponent<BarricadeControllerBeta>();
                placingBarricade = true;
            }
            else if (placingBarricade && barricadeBeingPlaced.IsValidPlacementLocation)
            {
                barricadeBeingPlaced.PlaceBarricade();
                placingBarricade = false;
                //placementTimer = placementDelay;

                // Add to current barricades
                currBarricades.Add(barricadeBeingPlaced.gameObject);
                numBarricades++;
            }
        }

        if (placingBarricade)
        {
            //cancel the barricade placement
            if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Cancel"))
            {
                placingBarricade = false;
                Destroy(barricadeBeingPlaced.gameObject);
            }
        }
    }


    void NestPlacement()
    {
        // Place barricades
        if (Input.GetKeyDown(KeyCode.L)) // || Input.GetButtonDown("PlaceNest")) -- not yet implemented
        {
            if (!placingBarricade && !placing && !placingNest && numNests < maxNests)
            {
                nestBeingPlaced = Instantiate(nestPrefab).GetComponent<PlacableNestControllerBeta>();
                placingNest = true;
            }
            else if (placingNest)
            {
                //TODO uncomment after fix
                nestBeingPlaced.PlaceNest();
                placingNest = false;

                // Add to current nests
                currNests.Add(nestBeingPlaced.gameObject);
                numNests++;
            }
        }

        if (placingNest)
        {
            //cancel the placement
            if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Cancel"))
            {
                placingNest = false;
                Destroy(nestBeingPlaced.gameObject);
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
        }
        else
        {
            Debug.LogError("Error: Must be a valid resource type");
        }

        // Make food refill stamina maybe?
        if (resourceType == 0)
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
        if (Cursor.lockState == CursorLockMode.Locked || Cursor.lockState == CursorLockMode.Confined)           // changed to allow confined cursor as well - SJ
        {
            float mouseVelX = Input.GetAxis("Mouse X");

            if (Mathf.Abs(mouseVelX) > 0.1f)     // Make a rotation deadzome to avoid unintended rotation
            {
                if (!Input.GetMouseButton(1) || Input.GetAxis("Zoom") > 0f)       // added zoom-in check to allow smooth rotations for zoom-in shooting - SJ
                {
                    float rotationDelta = (mouseVelX) * rotationSpeed * Time.deltaTime * 60f;
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + rotationDelta, 0f);
                }
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
        Vector3 animdir = new Vector3(direction.x, 0f, direction.z);      // Make sure you're not pointing up or down
        if (animdir.magnitude > 0.01f && isGrounded && !anim_isLanding)
        {
            if (!walkPlaying)
            {
                walkInstance = FMODUnity.RuntimeManager.CreateInstance("event:/characters/player/waddle");
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkInstance, GetComponent<Transform>(), GetComponent<Rigidbody>());
                walkInstance.start();
                walkPlaying = true;
            }
            animator.SetBool("IsWalking", true);
            animator.Play("Duck Walkcycle (handgun)");     // If there is movement, play the walk animation
        }
        else if (isGrounded && !anim_isLanding)
        {
            animator.SetBool("IsWalking", false);
            walkInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            walkPlaying = false;
            //animator.Play("Duck Idle (handgun)");
        }


        WalkInDirection(direction);
    }


    void Flap()
    {
        if (stamina >= staminaUsedPerJump)
        {
            animator.SetTrigger("Jumped");
            if (isGrounded)
            {
                animator.Play("Duck Depot or Jump (Handgun)");
            }
            else
            {
                animator.Play("Flap tap spacebar (Handgun)");
            }
            isGrounded = false;
            animator.SetBool("IsGrounded", isGrounded);
            rb.velocity = new Vector3(rb.velocity.x, flapSpeed, rb.velocity.z);
            stamina -= staminaUsedPerJump;
            flapTimer = flapDelay;
        }
    }

    protected void TurretLook()
    {
        GameObject selectedObject;
        //Ray ray;
        RaycastHit hitData;
        float maxDist = 10;
        //TODO: add check to see if player is close enough to the turret. Adjust the variable to be appropriate
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hitData, maxDist))
        {
            //tells the turret to Light up turret showing green, yellow or red for its states
            selectedObject = hitData.collider.gameObject;
            //Debug.Log(selectedObject.tag);
            if((selectedObject.CompareTag("Turret") || selectedObject.CompareTag("Flying Turret")))
            {
                PlaceableTurretControllerBeta turret = selectedObject.GetComponentInParent<PlaceableTurretControllerBeta>();
                //get the turret controller
                if (turret != null)
                {
                    //tell turret its being looked at
                    //turret.lookedAt(true);
                    lookedTurret = turret;
                    //tell the turret we are placing a duckling in it
                    if (ducklingToTurret &&  ducklingsList.Count > 0 && turret.AddDuckling())
                    {
                        //ducklingsList[0].ManTurret(turret);
                        ducklingsList[0].Die();
                        //ducklingsList.RemoveAt(0);

                    }
                    //or take one out
                    else if(removeDuckling && ducklingsList.Count < maxDucklings)
                    {
                        int removedDuckling = turret.RemoveDuckling();
                        if(removedDuckling == 0)
                        {
                            //generate new duck
                            StartCoroutine(SpawnDuckling(turret.constructionDelay, turret.transform.position));
                        }
                        else if(removedDuckling == 1)
                        {
                            //destroyed turret is removed from turrets, can place a new one
                            numTurrets--;
                        }
                        
                    }
                }
                return;
            }
            else if (selectedObject.tag == "Nest" && ducklingsList.Count > 0)
            {
                //get the turret controller
                if (selectedObject.TryGetComponent<NestControllerBeta>(out NestControllerBeta nest))
                {
                    //tell the nest we are placing a duckling in it
                    if (ducklingToTurret && nest.AddDuckling())
                    {
                        ducklingsList[0].ManNest(nest);
                        //ducklingsList[0].Die();
                        ducklingsList.RemoveAt(0);

                    }
                }
            }
        }
        //check turret is null
        if (lookedTurret != null)
        {
            //not null tell turret not looking anymore and 
            lookedTurret.lookedAt(false);
            lookedTurret = null;

        }
        
    }

    protected void NestLook()
    {
        GameObject selectedObject;
        //Ray ray;
        RaycastHit hitData;
        float maxDist = 10;
        //TODO lower the height of the ray
        //TODO: add check to see if player is close enough to the turret. Adjust the variable to be appropriate
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hitData, maxDist))
        {
            //tells the turret to Light up turret showing green, yellow or red for its states
            selectedObject = hitData.collider.gameObject;
            //Debug.Log(selectedObject.tag);
            if (selectedObject.tag == "Nest" && ducklingsList.Count > 0)
            {
                //get the turret controller
                if (selectedObject.TryGetComponent<NestControllerBeta>(out NestControllerBeta nest))
                {
                    //tell the nest we are placing a duckling in it
                    if (ducklingToTurret && nest.AddDuckling())
                    {
                        ducklingsList[0].ManNest(nest);
                        //ducklingsList[0].Die();
                        ducklingsList.RemoveAt(0);

                    }
                }
                return;
            }
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
        animator.SetTrigger("Recruiting");
        animator.Play("Duck Recruiting (handgun)");
        if(!recruitingSound)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/characters/player/recruit_quackling", GetComponent<Transform>().position);
            recruitingSound = true;
        }
        if (!recruitCircle.activeInHierarchy && ducklingsList.Count < maxDucklings)
        {
            recruitCircle.SetActive(true);
            
        }
        recruitCircle.transform.localPosition = new Vector3(0f, -0.9f, 0f);
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
        recruitingSound = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/characters/player/land_from_glide", GetComponent<Transform>().position);
        animator.SetBool("IsGrounded", isGrounded);
        animator.Play("Land (Handgun)");
        anim_isLanding = true;
        StartCoroutine(LandingTime());
    }

    public override void Die()
    {
        alive = false;
        DuckDeath();
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

    public void DucklingDied(DucklingControllerBeta deadling)
    {
        ducklingsList.Remove(deadling);
    }

    public void DuckDeath()
    {
        animator.SetTrigger("Dead");
        animator.Play("Duck Death");
        StartCoroutine(WaitToGameOver());
    }

    IEnumerator WaitToGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator LandingTime()
    {
        yield return new WaitForSeconds(0.5f);
        anim_isLanding = false;
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
                ducklingsList.Add(duckling_controller);
                duckling_controller.PlayQuack();
            }

        }
    }

    private IEnumerator SpawnDuckling(float delay, Vector3 turretPos)
    {
        yield return new WaitForSeconds(delay);
        
        Vector3 offset = UnityEngine.Random.onUnitSphere;                       // Random direction
        offset = new Vector3(offset.x, 0f, offset.z).normalized;    // Flatten and make the offset 1 unit long
        float spawnRadius = 5;
        float spawnHeight = 1;
        Vector3 spawnPosition = turretPos + (offset * spawnRadius) + (Vector3.up * spawnHeight);       // Make sure they don't spawn in the ground
        DucklingControllerBeta newDuck = Instantiate(ducklingPrefab, spawnPosition, Quaternion.identity).GetComponent<DucklingControllerBeta>();
        if(newDuck != null)
        {
            newDuck.animator = newDuck.gameObject.GetComponentInChildren<Animator>();
            newDuck.SetLeader(this);
            ducklingsList.Add(newDuck);
        }
    }
}
