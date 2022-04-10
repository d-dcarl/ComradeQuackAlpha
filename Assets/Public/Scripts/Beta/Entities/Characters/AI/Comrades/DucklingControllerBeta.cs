using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingControllerBeta : ComradeControllerBeta
{
    protected PlayerControllerBeta leader;
    protected NestControllerBeta homeNest;

    public Collider recruitRangeCollider;
    public Collider recruitCircleCollider;

    private PlaceableTurretControllerBeta turret;
    private bool isManning;

    public float wanderRadius;
    public float wanderTime;
    protected float wanderTimer;
    protected Vector3 wanderPos;

    private bool wasKilled = true;

    public Animator animator;

    public GameObject hats;

    public UnityEngine.UI.Image followingImage;

    public override void Start()
    {
        wasKilled = true;
        base.Start();
        turret = null;
        isManning = false;
        animator.Play("Quackling Idle");

        if (PlayerPrefs.GetInt("quackling_hat") > 0 && FindObjectOfType<CosmeticHandler>())
        {
            Debug.Log("Change hat");
            ChangeHat(FindObjectOfType<CosmeticHandler>().quacklingFittedHats[PlayerPrefs.GetInt("quackling_hat")]);
        }
        else
        {
            Debug.Log("no hat remove");
            RemoveHat();
        }

        //the following canvas
        followingImage.enabled = false;
        
    }

    public virtual void InitializeDuckling(NestControllerBeta nest)
    {
        if (nest == null)
        {
            Debug.LogError("Every duckling needs a home nest");
        }
        homeNest = nest;
        animator = this.gameObject.GetComponentInChildren<Animator>();
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
        followingImage.enabled = true;
        animator.SetTrigger("Saluting");
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
        if(isManning && turret != null)
        {
            if(!turret.alive)
            {
                this.gameObject.SetActive(true);
                this.Die();
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
        if(Vector3.Distance(transform.position, leader.transform.position) > followDistance)
        {
            WalkTowards(leader.gameObject);
        }
    }

    public virtual void Idle()
    {
        Vector3 toWanderPos = wanderPos - transform.position;
        if(toWanderPos.magnitude > 1f)
        {
            WalkInDirection(toWanderPos.normalized);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.Play("Quackling Idle");
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
            FollowLeader();
        }
        Shoot();
    }

    public void WalkTowards(GameObject target, GameObject lookTarget)
    {
        animator.SetBool("IsRunning", true);
        animator.Play("Quacking Running");
        WalkInDirection(target.transform.position - transform.position, lookTarget.transform.position - transform.position);
    }

    public override void Die()
    {
        if (wasKilled && homeNest != null)
        {
            homeNest.RemoveObject(gameObject);
            
        }
        if (leader != null)
        {
            leader.DucklingDied(this);
        }
        base.Die();
    }


    //TODO Make this not acutally kill the duckling, but just remove it from the scene and keep it in the turret
    public void ManTurret(PlaceableTurretControllerBeta turret)
    {
        wasKilled = false;
        isManning = true;
        //Destroy(this.gameObject);
        this.turret = turret;
        this.gameObject.SetActive(false);
    }

    //TODO Make this not acutally kill the duckling, but just remove it from the scene and keep it in the turret
    public void ManNest(NestControllerBeta nest)
    {
        wasKilled = false;
        isManning = true;
        //Destroy(this.gameObject);
        //this.turret = turret;
        this.gameObject.SetActive(false);
    }

    public void PlayQuack()
    {
        base.audioData.Play();
    }

    public void ChangeHat(GameObject newHat)
    {
        if (hats.transform.childCount > 0)
            Destroy(hats.transform.GetChild(0).gameObject);
        Instantiate(newHat, hats.transform);
    }

    public void RemoveHat()
    {
        if (hats.transform.childCount > 0)
            Destroy(hats.transform.GetChild(0).gameObject);
    }
}
