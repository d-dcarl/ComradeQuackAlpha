using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBeta : AIControllerBeta
{
    
    public HitboxControllerBeta attackHitBox;
    public int attackDamage;
    public float attackDelay;
    protected float attackTimer;

    protected GameObject source;
    protected GameObject destination;

    // TODO: Replace by creating a trigger hitbox for the pond's water mesh
    public float updateRadius = 4f;


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
            StyControllerBeta nearest = NearestSty();
            if(nearest != null)
            {
                if (source == null)
                {
                    FindNewSource();
                }
                float dist = Vector3.Distance(nearest.transform.position, transform.position);
            }
            
            if (source != null) {
                StyControllerBeta scb = source.GetComponent<StyControllerBeta>();
                if(scb != null)
                {
                    targetTransform = scb.GetTargetPond().transform;
                }
            }
        }
    }

    StyControllerBeta NearestSty()
    {
        StyControllerBeta nearest = null;

        if (GameManagerBeta.Instance != null)
        {
            
            foreach (PondControllerBeta pcb in GameManagerBeta.Instance.allStys)
            {
                if (pcb as StyControllerBeta != null)
                {
                    float checkDist = Vector3.Distance(pcb.transform.position, transform.position);
                    if (nearest == null || checkDist < Vector3.Distance(nearest.transform.position, transform.position))
                    {
                        nearest = pcb as StyControllerBeta;
                    }
                }
            }
        }

        return nearest;
    }

    void FindNewSource()
    {
        StyControllerBeta nearest = NearestSty();
        if(nearest != null)
        {
            source = nearest.gameObject;
        }
    }

    public void SetHomeSty(StyControllerBeta newHome)
    {
        source = newHome.gameObject;
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
