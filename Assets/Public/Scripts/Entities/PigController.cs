using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : EntityController
{
    [HideInInspector]
    public PondController homeSty;
    [HideInInspector]
    public EntityController target;
    [HideInInspector]
    public PondController targetPond;
    public float acceleration;
    public float maxSpeed;

    public HitboxController biteHB;
    public float attackDelay;
    public int attackDamage;
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

        ChooseTarget();
        if (target != null)
        {
            MoveTowardsTarget();
            Attack();
        }
        
    }

    public void ChooseTarget()
    {
        if(target == null)
        {
            if (homeSty == null)
            {
                // Placeholder. Eventually choose nearest sty.
                homeSty = GameManager.Instance.ponds[0];
            }
            if (homeSty.pointTo != null)
            {
                targetPond = homeSty.pointTo;
                target = targetPond;
            }
        }
        else if(GameManager.Instance != null)
        {
            GameObject player = GameManager.Instance.player;
            float playerDist = Vector3.Distance(player.transform.position, transform.position);
            float targetDist = Vector3.Distance(target.transform.position, transform.position);

            if (playerDist < targetDist)
            {
                target = player.GetComponent<EntityController>();
            }
        }
    }

    public void MoveTowardsTarget()
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
    }
    public void Attack()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (biteHB.tracked.Contains(target.gameObject))
        {
            target.TakeDamage(attackDamage);
            attackTimer = attackDelay;
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
                if(homeSty.pointTo != null)
                {
                    target = homeSty.pointTo;
                }
            }
        }
    }
}
