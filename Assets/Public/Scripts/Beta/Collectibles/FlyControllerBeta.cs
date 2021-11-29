using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControllerBeta : CollectibleControllerBeta
{
    public float wanderRadius;
    public float wanderSpeed;
    public float wanderAcceleration;
    public float wanderTime;
    protected float wanderTimer;

    protected Vector3 startingPos;
    protected Vector3 wanderPos;

    protected Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError("Fly needs a rigid body");
        }

        startingPos = transform.position;
        SetWanderPos();
    }

    public void Update()
    {
        Wander();
        if(wanderTimer <= 0f)
        {
            wanderTimer = wanderTime;
            SetWanderPos();
        }
    }

    public void Wander()
    {
        Vector3 toWanderPos = (wanderPos - transform.position).normalized;
        rb.AddForce(toWanderPos * wanderAcceleration * Time.deltaTime * 60f);   // Account for framerate and scale to offset the effect
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, wanderSpeed);     // Enforce max speed
        wanderTimer -= Time.deltaTime;
    }

    public void SetWanderPos()
    {
        Vector2 randomVec = Random.insideUnitCircle * wanderRadius;
        wanderPos = startingPos + new Vector3(randomVec.x, 0f, randomVec.y);
        wanderTimer = wanderTime;
    }
}
