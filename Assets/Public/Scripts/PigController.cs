using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    [HideInInspector]
    public PondController target;
    public float acceleration;
    public float maxSpeed;

    public float attackDelay;
    public float attackRadius;
    public int attackDamage;
    public float health = 10;
    private float attackTimer;

    private Rigidbody rb;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;


        SelectTarget();


        attackTimer = attackDelay;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            this.enabled = false;
            Destroy(this.gameObject);
        }
        if(target != null && !target.isSty)
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


            if(attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            } else if((target.transform.position - transform.position).magnitude <= attackRadius) 
            {
                target.TakeDamage(attackDamage);
                attackTimer = attackDelay;
            }
        } else
        {
            SelectTarget();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Debug.Log("Health: " + health);
            health -= other.gameObject.GetComponent<BulletController>().damage;
        }
    }

    void SelectTarget()
    {
        // TODO: Pathfind to nearest pond
        if (gm.ponds.Count > 0)
        {
            foreach (PondController pc in gm.ponds)
            {
                if (!pc.isSty)
                {
                    target = pc;
                }
            }
        }
    }
}
