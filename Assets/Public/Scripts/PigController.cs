using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigController : MonoBehaviour
{
    [HideInInspector]
    public PondController target;
    public float speed;

    public float attackDelay;
    public float attackRadius;
    public int attackDamage;
    public float health = 10;
    private float attackTimer;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.ponds[0];
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
        if(target != null)
        {
            Vector3 toTarget = target.transform.position - transform.position;
            toTarget.y = 0;
            toTarget = toTarget.normalized;
            rb.AddForce(toTarget * speed, ForceMode.Force);

            if(attackTimer > 0f)
            {
                attackTimer -= Time.deltaTime;
            } else if((target.transform.position - transform.position).magnitude <= attackRadius) 
            {
                target.TakeDamage(attackDamage);
                attackTimer = attackDelay;
            }
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
}
