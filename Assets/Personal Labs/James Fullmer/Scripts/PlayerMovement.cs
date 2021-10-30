using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float jumpForce = 100f;
    public float glideGravityCap = 2f;
    public float glideSpeed = 10f;
    public int numberOfJumps = 3;
    public Transform cam;

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
        if (/*currentJumps < numberOfJumps && */Input.GetButtonDown("Jump"))
        {
            //Debug.Log("I jump");
            if (isMounted && mountTrigger)
            {
                isMounted = false;
                mountTrigger.Dismount();
                mountTrigger = null;
            }
            isGrounded = false;
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

        if (!isGrounded)
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

    }

    void FixedUpdate()
    {

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
                //rb.useGravity = false;
                //rb.AddForce(Physics.gravity * rb.mass * .5f);
                if (rb.velocity.y < -glideGravityCap)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -glideGravityCap, rb.velocity.z);
                }
                anim.SetBool("isGliding", true);
            }
            else
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
    private void LateUpdate()
    {
        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.05f))
        {
            //Debug.Log("Is grounded");
            currentJumps = 0;
            isGrounded = true;
        }
    }

    public void MountDuck(Transform mount, MountTrigger mt)
    {
        isMounted = true;
        mountTrigger = mt;
        this.mount = mount;
    }
}
