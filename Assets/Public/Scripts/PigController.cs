using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    [HideInInspector]
    public PondController homeSty;
    public PondController target;
    public float acceleration;
    public float maxSpeed;

    public float attackDelay;
    public float attackRadius;
    public int attackDamage;
    public float health = 10;
    private float attackTimer;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // SelectTarget();


        attackTimer = attackDelay;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log("Pig target: " + target);
        Debug.Log("Pig home Sty: " + homeSty);
        Debug.Log("Pig home Sty point to: " + homeSty.pointTo);
        */

        if (target != null)
        {
            // Look at target but stay horizontal
            transform.LookAt(target.transform);
            Vector3 angles = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0f, angles.y, angles.z);

            // Apply force in direction of target
            Vector3 toTarget = target.transform.position - transform.position;
            toTarget.y = 0;
            toTarget = toTarget.normalized;
            rb.AddForce(toTarget * acceleration * Time.deltaTime, ForceMode.Force);

            // Enfore max speed only in x and z direction.
            Vector3 flatSpeed = rb.velocity;
            flatSpeed.y = 0f;
            flatSpeed = Vector3.ClampMagnitude(flatSpeed, maxSpeed);
            rb.velocity = new Vector3(flatSpeed.x, rb.velocity.y, flatSpeed.z);

            if(!target.isSty)
            {
                if (attackTimer > 0f)
                {
                    attackTimer -= Time.deltaTime;
                }
                else if ((target.transform.position - transform.position).magnitude <= attackRadius)
                {
                    target.TakeDamage(attackDamage);
                    attackTimer = attackDelay;
                }
            }
        } else
        {
            if(GameManager.Instance != null)
            {
                if (homeSty == null)
                {
                    // Placeholder. Eventually choose nearest sty.
                    homeSty = GameManager.Instance.ponds[0];
                }
                target = homeSty.pointTo;
            }
            
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Pond"))
        {
            PondController pondTouched = other.GetComponent<PondController>();
            if (pondTouched.isSty)
            {
                homeSty = pondTouched;
                target = homeSty.pointTo;
            }
        }
    }
}
