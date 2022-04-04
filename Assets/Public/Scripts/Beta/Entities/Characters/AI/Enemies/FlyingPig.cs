using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPig : EnemyControllerBeta
{
    public Transform turret;
    public GameObject projectile;
    public float startHeight;

    public override void Start()
    {
        base.Start();
        transform.position = new Vector3(transform.position.x, transform.position.y + startHeight, transform.position.z);
    }

    public override void Update()
    {
        if (currentHealth <= 0 || transform.position.y < 0f)
        {
            Die();
        }
        CleanTouchingList();
        ChooseTarget();
        FollowTarget();
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        if (attackTimer <= 0f)
        {
            if (targetHitbox.tracked.Count > 0)
            {
                Attack();
                attackTimer = attackDelay;
            }
        }
    }

    public override void Attack()
    {
        GameObject g = ClosestInRange();
        if (g)
        {
            SpawnProjectile(g);
        }
        //foreach (GameObject g in targetHitbox.tracked)
        //{
        //    if (g != null)
        //    {
        //        // Maybe implement a CanDamage list at some point
        //        if (canAttack.Contains(g.tag))
        //        {
        //            Debug.Log("Fire!");
        //            SpawnProjectile();
        //            break;
        //        }
        //    }
        //}
    }

    //public override void PointInDirectionXZ(Vector3 direction)
    //{
    //    if (Vector3.Distance(transform.position, direction) > 0.5f)
    //        transform.forward = new Vector3(direction.x, 0f, direction.z).normalized;
    //}
    public override void WalkInDirection(Vector3 direction)
    {
        var flatDirection = new Vector3(direction.x, 0, direction.z);
        if (flatDirection.magnitude > 30)
        {
            base.WalkInDirection(direction);
            PointInDirectionXZ(direction);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        turret.forward = -direction;
    }

    void SpawnProjectile(GameObject targ)
    {
        BulletControllerBeta proj = Instantiate(projectile).GetComponent<BulletControllerBeta>();
        proj.transform.position = turret.transform.position;
        proj.direction = targ.transform.position - turret.position;
    }
}
