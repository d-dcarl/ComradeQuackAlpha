using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerBeta : CharacterControllerBeta
{
    public RangeHitboxControllerBeta targetHitbox;
    public List<string> canAttack;

    protected List<GameObject> touching;
    protected Transform targetTransform;

    public override void Start()
    {
        base.Start();
        touching = new List<GameObject>();
    }

    public override void Update()
    {
        base.Update();
        CleanTouchingList();
    }

    public virtual void ChooseTarget()
    {
        targetTransform = null;

        GameObject closest = ClosestInRange();
        if (closest != null)
        {
            if (!TouchingTarget(closest))
            {
                targetTransform = closest.transform;
            }
        }
        
    }

    public virtual void FollowTarget()
    {
        if(targetTransform != null)
        {
            WalkInDirection(targetTransform.position - transform.position);
        }
    }

    public virtual bool TouchingTarget(GameObject target)
    {
        return touching.Contains(target);
    }

    // Keep track of which objects you are touching
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!touching.Contains(collision.gameObject))
        {
            touching.Add(collision.gameObject);
        }
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        if (touching.Contains(collision.gameObject))
        {
            touching.Remove(collision.gameObject);
        }
    }

    public GameObject ClosestInRange()
    {
        GameObject closest = null;
        foreach (GameObject g in targetHitbox.tracked)
        {
            if (g != null && canAttack.Contains(g.tag))
            {
                // Only target living entities even if they don't get destroyed when they die
                EntityControllerBeta ecb = g.GetComponent<EntityControllerBeta>();
                if(ecb != null && ecb.alive)
                {
                    float dist = Vector3.Distance(transform.position, g.transform.position);
                    if (closest == null || dist < Vector3.Distance(closest.transform.position, transform.position))
                    {
                        closest = g;
                    }
                }
            }
        }
        return closest;
    }

    public void CleanTouchingList()
    {
        int i = 0;
        while (i < touching.Count)
        {
            if (touching[i] == null)
            {
                touching.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
}
