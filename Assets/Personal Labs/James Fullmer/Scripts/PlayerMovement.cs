using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public
    [Header("Speed")]
    public float speed = 6f;
    public float sprintSpeed = 12f;
    [Header("Jump and Gliding")]
    public float jumpForce = 100f;
    public float glideGravityCap = 2f;
    public float glideSpeed = 10f;
    [Tooltip("Currently not used")]
    public int numberOfJumps = 3;
    [Header("Stamina")]
    public float stamina = 100f;
    public float staminaRecovery = 10f;
    public float staminaUsedPerJump = 10f;
    public float staminaUsedPerGlideSecond = 5f;
    [Header("Other")]
    public Transform cam; //Camera Holder
    public Transform respawnPosition;
    public GameObject dialogueManager;
    public bool enterDialogue = false;

    [Tooltip("Used to smoothly rotate when moving in a different direction")]
    public float turnSmoothTime = 0.1f;
    public Animator anim;

    public bool isMounted = false;

    //movespeed can be seen by other classes, but can only be set here
    public float moveSpeed { get; private set; }

    //Private

    Rigidbody rb;
    bool isGrounded;
    int currentJumps;
    bool isZoomedIn = false;
    Transform mount;
    MountTrigger mountTrigger;

    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        currentJumps = 0;
        numberOfJumps--;
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        //When jumped and has stamina
        if (/*currentJumps < numberOfJumps && */Input.GetButtonDown("Jump") && stamina >= staminaUsedPerJump)
        {
            //Debug.Log("I jump");
            //If you jump and is mounted on the bear
            if (isMounted && mountTrigger)
            {
                isMounted = false;
                mountTrigger.Dismount();
                mountTrigger = null;
            }
            isGrounded = false;
            //The force that is added when jumped
            rb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
            currentJumps++;
            stamina -= staminaUsedPerJump;
            anim.Play("flap");
        }
        //Zooming in
        if (Input.GetButton("Fire2"))
        {
            isZoomedIn = true;
        }
        else //Not zoomed in
        {
            isZoomedIn = false;
        }

        //If you are in the air and it's not your first jump
        if (!isGrounded && currentJumps > 1)
        {
            moveSpeed = glideSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = speed;
        }
        //Debug.Log(stamina);
      

    }

    //Used to handle movement, gliding, and general physics
    void FixedUpdate()
    {
        if (!GameManager.Instance.isOverheadView)
        {

            //If you are not mounted
            if (!isMounted)
            {
                //Movement
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(h, 0, v).normalized;
                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    if (!isZoomedIn)
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    rb.MovePosition(transform.position + moveDir.normalized * Time.deltaTime * moveSpeed);
                }
                //If jump is held down and stamina is greater than zero
                if (Input.GetButton("Jump") && stamina > 0)
                {
                    //rb.useGravity = false;
                    //rb.AddForce(Physics.gravity * rb.mass * .5f);
                    if (rb.velocity.y < -glideGravityCap)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, -glideGravityCap, rb.velocity.z);
                    }
                    stamina -= staminaUsedPerGlideSecond * Time.deltaTime;
                    anim.SetBool("isGliding", true);
                }
                else //If you are not gliding
                {
                    rb.useGravity = true;
                    anim.SetBool("isGliding", false);

                }
            }
            if (isMounted)
            {
                transform.position = mount.position;
                if (!isZoomedIn)
                    transform.rotation = mount.rotation;
            }

            var velocity = this.GetComponent<Rigidbody>().velocity;
            this.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x * 0.99f, velocity.y, velocity.z * 0.99f);
        }
    }
    //Used to calculate if on the ground
    private void LateUpdate()
    {
        //Calculates if you are on the ground
        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.05f))
        {
            //Debug.Log("Is grounded");
            currentJumps = 0;
            isGrounded = true;
            if (stamina < 100)
                stamina += staminaRecovery * Time.deltaTime;

            
        }
        if (enterDialogue)
        {
            if (Input.GetButtonDown("Enter"))
            {
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence();
            }
        }

    }

    /// <summary>
    /// Calls this if you are mounting the bear
    /// </summary>
    /// <param name="mount">The bear</param>
    /// <param name="mt">The Mount Trigger script to call later to dismount</param>
    public void MountDuck(Transform mount, MountTrigger mt)
    {
        isMounted = true;
        mountTrigger = mt;
        this.mount = mount;
    }
    /// <summary>
    /// If player dies, respawn back at respawn position
    /// </summary>
    public void playerDeath()
    {
        //If there is a respawn position
        if (respawnPosition)
            transform.position = respawnPosition.position;
    }

}
