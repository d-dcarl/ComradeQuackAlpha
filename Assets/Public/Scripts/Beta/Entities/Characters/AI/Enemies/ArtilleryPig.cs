using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtilleryPig : EnemyControllerBeta
{
    public Slider progressSlider;
    public float maxWaitTime;
    float waitTimeCounter = 0;
    public GameObject projectile;
    public float xForce;
    public float yForce;
    public Transform cannon;
    //public Transform targetBox;

    
    public bool canMove = true;
    public bool isProgressing = false;
    public bool isFiring = false;
    public bool isPackingUp = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        progressSlider.maxValue = maxWaitTime;
        progressSlider.value = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (isProgressing && !isFiring)
        {
            waitTimeCounter += Time.deltaTime;
            progressSlider.value = waitTimeCounter;
            if (waitTimeCounter >= maxWaitTime)
            {
                progressSlider.value = 0;
                isFiring = true;
                waitTimeCounter = 0;
            }
        }
        else if (isPackingUp && !canMove && !isFiring)
        {
            waitTimeCounter += Time.deltaTime;
            progressSlider.value = waitTimeCounter;
            if (waitTimeCounter >= maxWaitTime)
            {
                progressSlider.value = 0;
                isPackingUp = false;
                canMove = true;
                waitTimeCounter = 0;
            }
        }
    }
    public override void FollowTarget()
    {
        if (targetTransform != null && canMove)
        {
            WalkInDirection(targetTransform.position - transform.position);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    //public override void WalkInDirection(Vector3 direction)
    //{
    //    if (canMove)
    //    {
    //        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);      // Make sure you're not pointing up or down
    //        if (flatDirection.magnitude <= 0.01f)
    //        {
    //            return;     // If there is no movement, skip the next steps
    //        }
    //        flatDirection = flatDirection.normalized;       // Just the direction, so scale the vector to length 1
    //        rb.AddForce(flatDirection * acceleration * Time.deltaTime * 60f);           // Account for framerate
    //        Vector3 flatSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Your speed in only horizontal directions
    //        Vector3 boundedSpeed = Vector3.ClampMagnitude(flatSpeed, maxSpeed);
    //        rb.velocity = new Vector3(boundedSpeed.x, rb.velocity.y, boundedSpeed.z);   // Keep the old y speed, but use the clamped x and z
    //    }
    //}

    public override void Attack()
    {
        bool canHit = false;
        foreach (GameObject g in attackHitBox.tracked)
        {
            if (g != null)
            {
                // Maybe implement a CanDamage list at some point
                if (canAttack.Contains(g.tag))
                {
                    if (g.GetComponent<EntityControllerBeta>().alive)
                    {
                        canHit = true;
                        isProgressing = true;
                        canMove = false;
                        break;
                    }
                    
                }
            }
        }
        if (!canHit && isFiring)
        {
            isPackingUp = true;
            isFiring = false;
            isProgressing = false;
        }

        if (isFiring)
        {
            foreach (GameObject g in attackHitBox.tracked)
            {
                if (g != null)
                {
                    // Maybe implement a CanDamage list at some point
                    if (canAttack.Contains(g.tag))
                    {
                        if (g.GetComponent<EntityControllerBeta>().alive)
                        {
                            Debug.Log("Fire at " + g.name);
                            transform.LookAt(g.transform);
                            GameObject proj = Instantiate(projectile, cannon.position, cannon.rotation);
                            float dist = Vector3.Distance(cannon.position, g.transform.position);
                            proj.GetComponent<Rigidbody>().velocity = new Vector3(cannon.forward.x * xForce * dist / 2, yForce, cannon.forward.z * xForce * dist / 2);
                            proj.GetComponent<ArtilleryShell>().target = g.transform;
                            Debug.Log(proj.name);
                        }
                    }
                }
            }
        }
        
    }
}
