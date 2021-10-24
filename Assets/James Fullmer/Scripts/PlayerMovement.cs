using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public
    [SerializeField] public GameObject placeableTurret;
    [SerializeField] public float placementCooldown = 5.0f;
    [SerializeField] public float placementDistance = 2.0f;
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float jumpForce = 100f;
    public int numberOfJumps = 3;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    public Animator anim;

    public bool isMounted = false;


    //Private
    private float cooldown = 0;
    
    Rigidbody rb;
    //bool isGrounded;
    int currentJumps;
    bool isZoomedIn = false;
    float moveSpeed;
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
        if (/*currentJumps < numberOfJumps && */Input.GetButtonDown("Jump"))
        {
            //Debug.Log("I jump");
            if (isMounted && mountTrigger)
            {
                isMounted = false;
                mountTrigger.Dismount();
                mountTrigger = null;
            }
            rb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
            currentJumps++;
            anim.Play("flap");
        }

        if (Input.GetButton("Fire2"))
        {
            isZoomedIn = true;
        }
        else
        {
            isZoomedIn = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = speed;
        }
        
        //Input for placing a turret
        if(Input.GetKey(KeyCode.E))
        {
            if(cooldown <= 0)
            {
                Vector3 newPosition = new Vector3(transform.position.x + (placementDistance * this.transform.forward.x), transform.position.y, transform.position.z +(placementDistance * this.transform.forward.z));
                Instantiate<GameObject>(placeableTurret, newPosition, this.transform.rotation);
                cooldown = placementCooldown;
            }
                
        }

    }

    void FixedUpdate()
    {
        //update placement cooldown
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        else
        { 
            cooldown -= Time.deltaTime;
        }
        

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

            if (Input.GetButton("Jump"))
            {
                rb.useGravity = false;
                rb.AddForce(Physics.gravity * rb.mass * .5f);
            }
            else
            {
                rb.useGravity = true;
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
    private void LateUpdate()
    {
        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.05f))
        {
            Debug.Log("Is grounded");
            currentJumps = 0;
        }
    }

    public void MountDuck(Transform mount, MountTrigger mt)
    {
        isMounted = true;
        mountTrigger = mt;
        this.mount = mount;
    }
}
