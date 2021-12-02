using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingControllerBeta : ComradeControllerBeta
{
    protected PlayerControllerBeta leader;
    protected NestControllerBeta homeNest;

    public Collider recruitRangeCollider;
    public Collider recruitCircleCollider;

    public float wanderRadius;
    public float wanderTime;
    protected float wanderTimer;
    protected Vector3 wanderPos;

    public virtual void InitializeDuckling(NestControllerBeta nest)
    {
        if (nest == null)
        {
            Debug.LogError("Every duckling needs a home nest");
        }
        homeNest = nest;
        SetWanderPos();
    }

    // Not being used yet. Use it for recruiting
    public virtual PlayerControllerBeta GetLeader()
    {
        return leader;
    }

    public virtual void SetLeader(PlayerControllerBeta newLeader)
    {
        leader = newLeader;
    }

    public override void Update()
    {
        base.Update();
        if (currentTarget == null)
        {
            if (leader == null)
            {
                Idle();
            } else
            {
                FollowLeader();
            }
        }
    }

    public override ProjectileControllerBeta Shoot()
    {
        BulletControllerBeta bcb = base.Shoot() as BulletControllerBeta;
        if(bcb != null)
        {
            bcb.direction = transform.forward;
        }

        return bcb;
    }

    public virtual void FollowLeader()
    {
        // Placeholder. Fill in with comrade AI.
        WalkTowards(leader.gameObject);
        //Debug.Log("Tee dum, tee dee, a teedly do tee day");
    }

    public virtual void Idle()
    {
        Vector3 toWanderPos = wanderPos - transform.position;
        if(toWanderPos.magnitude > 1f)
        {
            WalkInDirection(toWanderPos.normalized);
        }
        
        wanderTimer -= Time.deltaTime;
        if(wanderTimer <= 0f)
        {
            SetWanderPos();
        }
    }

    public virtual void SetWanderPos()
    {
        Vector2 randomPos = Random.insideUnitCircle * wanderRadius;
        wanderPos = homeNest.transform.position + new Vector3(randomPos.x, 0f, randomPos.y);
        wanderTimer = wanderTime;
    }

    public override void AttackTarget(GameObject target)
    {
        if(leader == null)
        {
            WalkTowards(target);
        }
        else
        {
            WalkTowards(leader.gameObject, target);
            
        }
        Shoot();
    }

    public void WalkTowards(GameObject target, GameObject lookTarget)
    {
        WalkInDirection(target.transform.position - transform.position, lookTarget.transform.position - transform.position);
    }

    public override void Die()
    {
        homeNest.RemoveObject(gameObject);
        if (leader != null)
        {
            leader.DucklingDied(this);
        }
        base.Die();
    }

    public void ManTurret()
    {
        base.Die();
    }

    public void PlayQuack()
    {
        base.audioData.Play();
    }
}
