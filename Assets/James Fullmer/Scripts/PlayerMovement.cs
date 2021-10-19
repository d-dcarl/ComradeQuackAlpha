using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public
    public float speed = 6f;
    public float jumpForce = 100f;
    public int numberOfJumps = 3;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    public Animator anim;

    //Private
    Rigidbody rb;
    bool isGrounded;
    int currentJumps;
    bool isZoomedIn = false;

    float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        isGrounded = true;
        currentJumps = 0;
        numberOfJumps--;
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (currentJumps < numberOfJumps && Input.GetButtonDown("Jump"))
        {
            Debug.Log("I jump");
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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
    }

    void FixedUpdate()
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
            rb.MovePosition(transform.position + moveDir.normalized * Time.deltaTime * speed);
        }
        
        
    }
    private void LateUpdate()
    {
        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.05f))
        {
            Debug.Log("Is grounded");
            isGrounded = true;
            currentJumps = 0;
        }
    }
}
