using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComradeControllerBeta : AIControllerBeta
{
    public GameObject projectilePrefab;
    public float shotDelay;
    protected float shotTimer;

    public override void Start()
    {
        base.Start();
        if(projectilePrefab.GetComponent<ProjectileControllerBeta>() == null)
        {
            Debug.LogError("Error: Projectile does not have a projectile controller beta attached.");
        }
        shotTimer = shotDelay;
    }

    public override void Update()
    {
        base.Update();
        GameObject currentTarget = ClosestInRange();
        if(currentTarget == null)
        {
            Idle();
        } else
        {
            AttackTarget(currentTarget);
        }
    }

    public virtual void AttackTarget(GameObject target)
    {
        WalkTowards(target);
        Shoot();
    }

    public virtual ProjectileControllerBeta Shoot()
    {
        shotTimer -= Time.deltaTime;
        if(shotTimer <= 0f)
        {
            shotTimer = shotDelay;
            ProjectileControllerBeta pcb = Instantiate(projectilePrefab).GetComponent<ProjectileControllerBeta>();
            pcb.transform.position = transform.position;
            return pcb;
        }
        return null;
    }

    // Make sure you point towards whatever you're following
    public override void WalkInDirection(Vector3 direction)
    {
        base.WalkInDirection(direction);
        PointInDirectionXZ(direction);
    }

    public virtual void Idle()
    {

    }
}
