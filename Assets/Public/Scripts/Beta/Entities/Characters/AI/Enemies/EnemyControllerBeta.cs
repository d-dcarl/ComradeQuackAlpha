using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBeta : AIControllerBeta
{
    
    public HitboxControllerBeta attackHitBox;
    public int attackDamage;
    public float attackDelay;
    protected float attackTimer;

    public float targetUpdateTime;
    protected float targetUpdateTimer;

    public float updateRadius = 5f;

    protected static List<EnemyControllerBeta> allEnemies;

    protected NodeControllerBeta source;
    public bool isTutorial;

    public override void Start()
    {
        if(allEnemies == null)
        {
            allEnemies = new List<EnemyControllerBeta>();
        }
        allEnemies.Add(this);

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
        targetUpdateTimer = targetUpdateTime;
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
        targetUpdateTimer -= Time.deltaTime;
        if(targetUpdateTimer <= 0f)
        {
            targetUpdateTimer = targetUpdateTime;
            base.ChooseTarget();

            if (targetTransform == null)
            {
                NodeControllerBeta nextNode = FindNextNode();
                if (nextNode != null && !isTutorial)
                {
                    targetTransform = nextNode.transform;
                }
            }
        }
    }

    public void UpdateSource()
    {
        NodeControllerBeta closest = FindClosestNode();
        if (source == null)
        {
            source = closest;
        }
        else if (source != closest)
        {
            // Vector with x and z of closest node, but y of self
            Vector3 closestAtY = new Vector3(closest.transform.position.x, transform.position.y, closest.transform.position.z);
            float flatDist = Vector3.Distance(closestAtY, transform.position);
            if(flatDist < updateRadius)
            {
                source = closest;
            }
        }
    }

    public NodeControllerBeta FindNextNode()
    {
        UpdateSource();

        NodeControllerBeta checking = source;
        NodeControllerBeta dest = null;

        // If any neighbors are goals
        foreach (NodeControllerBeta n in checking.neighbors)
        {
            if(n.isGoal)
            {
                float nDist = Vector3.Distance(n.transform.position, transform.position);
                if (dest == null || nDist < Vector3.Distance(dest.transform.position, transform.position))
                {
                    dest = n;
                }
            }
        }

        // Otherwise pick the best non-goal neighbor
        if (dest == null)
        {
            NodeControllerBeta goal = FindClosestGoal();
            float bestDist = -1f;
            foreach (NodeControllerBeta n in checking.neighbors)
            {
                float nDist = Vector3.Distance(n.transform.position, transform.position);
                float gDist = Vector3.Distance(n.transform.position, goal.transform.position);

                if(bestDist < 0f || nDist + gDist < bestDist)
                {
                    bestDist = nDist + gDist;
                    dest = n;
                }
            }
        }

        return dest;
    }

    // Returns null if nodes are not set up
    public NodeControllerBeta FindClosestNode()
    {
        NodeControllerBeta closest = null;
        float closestDist = -1;

        // If the nodes are set up
        if(GameManagerBeta.Instance != null && GameManagerBeta.Instance.allNodes != null)
        {
            foreach(NodeControllerBeta node in GameManagerBeta.Instance.allNodes)
            {
                float dist = Vector3.Distance(transform.position, node.transform.position);
                if (closest == null || dist < closestDist)
                {
                    closestDist = dist;
                    closest = node;
                }
            }
        }

        return closest;
    }

    public NodeControllerBeta FindClosestGoal()
    {
        NodeControllerBeta closest = null;
        float closestDist = -1;

        // If the nodes are set up
        if (GameManagerBeta.Instance != null && GameManagerBeta.Instance.allNodes != null)
        {
            foreach (NodeControllerBeta node in GameManagerBeta.Instance.allNodes)
            {
                if(node.isGoal)
                {
                    float dist = Vector3.Distance(transform.position, node.transform.position);
                    if (closest == null || dist < closestDist)
                    {
                        closestDist = dist;
                        closest = node;
                    }
                }
            }
        }

        return closest;
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
                    if (ecb != null)
                        ecb.TakeDamage(attackDamage);
                }
            }
        }
    }

    public override void Die()
    {
        allEnemies.Remove(this);
        base.Die();
    }

    public static int enemyCount()
    {
        if(allEnemies == null)
        {
            allEnemies = new List<EnemyControllerBeta>();
        }

        int i = 0;
        while(i < allEnemies.Count)
        {
            if(allEnemies[i] == null)
            {
                allEnemies.RemoveAt(i);
            } else
            {
                i++;
            }
        }
        return allEnemies.Count;
    }
}
