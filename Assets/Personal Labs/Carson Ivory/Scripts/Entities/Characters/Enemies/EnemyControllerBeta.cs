using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBeta : CharacterControllerBeta
{
    public List<string> canAttack;
    public HitboxControllerBeta attackHitBox;
    public int attackDamage;
    public float attackDelay;
    protected float attackTimer;

    public RangeHitboxControllerBeta followHitbox;

    public override void Start()
    {
        base.Start();
        attackTimer = attackDelay;
        if (attackHitBox == null)
        {
            Debug.LogError("Error: Make sure the enemy has an attack hitbox");
        }
        else
        {
            BoxCollider bc = attackHitBox.GetComponent<BoxCollider>();
            if (bc == null || !bc.GetComponent<BoxCollider>().isTrigger)
            {
                Debug.LogError("Error: Make sure your attack hitbox has a trigger collider");
            }
        }
    }

    public override void Update()
    {
        base.Update();
        FollowTarget();
        if(attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        if(attackTimer <= 0f)
        {
            if(attackHitBox.tracked.Count > 0)
            {
                Attack();
                attackTimer = attackDelay;
            }
        }
    }

    public void FollowTarget()
    {
        GameObject closest = ClosestInRange();
        if (closest != null)
        {
            if(!attackHitBox.tracked.Contains(closest))
            {
                WalkInDirection(closest.transform.position - transform.position);
            }
        }
    }

    public GameObject ClosestInRange()
    {
        GameObject closest = null;
        foreach (GameObject g in followHitbox.tracked)
        {
            if (g != null && canAttack.Contains(g.tag))
            {
                float dist = Vector3.Distance(transform.position, g.transform.position);
                if (closest == null || dist < Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = g;
                }
            }
        }
        return closest;
    }

    public override void WalkInDirection(Vector3 direction)
    {
        base.WalkInDirection(direction);
        transform.forward = new Vector3(direction.x, 0f, direction.z).normalized;   // Whatever direction you're walking, but not up or down.
    }

    public virtual void Attack()
    {
        foreach(GameObject g in attackHitBox.tracked)
        {
            if(g != null)
            {
                // Maybe implement a CanDamage list at some point
                if (canAttack.Contains(g.tag))
                {
                    EntityControllerBeta ecb = g.GetComponent<EntityControllerBeta>();
                    ecb.TakeDamage(attackDamage);
                }
            }
        }
    }
}
