using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBeta : AIControllerBeta
{
    
    public HitboxControllerBeta attackHitBox;
    public int attackDamage;
    public float attackDelay;
    protected float attackTimer;

    protected StyControllerBeta homeSty;


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
        ChooseTarget();
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

    public override void ChooseTarget()
    {
        base.ChooseTarget();
        if(targetTransform == null)
        {
            SetHomeSty();
            if (homeSty != null) {
                targetTransform = homeSty.GetTargetPond().transform;
            }
        }
    }

    void SetHomeSty()
    {
        if(GameManagerBeta.Instance != null)
        {
            StyControllerBeta newHome = null;
            foreach(PondControllerBeta pcb in GameManagerBeta.Instance.allStys)
            {
                if(pcb as StyControllerBeta != null)
                {
                    float checkDist = Vector3.Distance(pcb.transform.position, transform.position);
                    if(newHome == null || checkDist < Vector3.Distance(newHome.transform.position, transform.position))
                    {
                        newHome = pcb as StyControllerBeta;
                    }
                }
            }
            homeSty = newHome;
        }
    }

    public void SetHomeSty(StyControllerBeta newHome)
    {
        homeSty = newHome;
    }

    public override bool TouchingTarget(GameObject target)
    {
        return base.TouchingTarget(target) || attackHitBox.tracked.Contains(target);
    }

    public override void WalkInDirection(Vector3 direction)
    {
        base.WalkInDirection(direction);
        PointInDirectionXZ(direction);
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
