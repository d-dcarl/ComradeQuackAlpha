using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBeta : EntityControllerBeta
{
    protected Rigidbody rb;

    public float acceleration;
    public float maxSpeed;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.Log("No rigidbody found for character " + gameObject.name);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public virtual void WalkInDirection(Vector3 direction)
    {
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);      // Make sure you're not pointing up or down
        if(flatDirection.magnitude <= 0f)
        {
            return;     // If there is no movement, skip the next steps
        }
        flatDirection = flatDirection.normalized;       // Just the direction, so scale the vector to length 1
        rb.AddForce(flatDirection * acceleration * Time.deltaTime * 60f);           // Account for framerate
        EnforceMaxSpeed();
    }

    void EnforceMaxSpeed()
    {
        Vector3 flatSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Your speed in only horizontal directions
        Vector3 boundedSpeed = Vector3.ClampMagnitude(flatSpeed, maxSpeed);
        rb.velocity = new Vector3(boundedSpeed.x, rb.velocity.y, boundedSpeed.z);   // Keep the old y speed, but use the clamped x and z
    }
}
