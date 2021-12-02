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

    public virtual void WalkInDirection(Vector3 direction)
    {
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);      // Make sure you're not pointing up or down
        if(flatDirection.magnitude <= 0.01f)
        {
            return;     // If there is no movement, skip the next steps
        }
        flatDirection = flatDirection.normalized;       // Just the direction, so scale the vector to length 1
        rb.AddForce(flatDirection * acceleration * Time.deltaTime * 60f);           // Account for framerate
        EnforceMaxSpeed();
    }

    public virtual void WalkTowards(GameObject target)
    {
        WalkInDirection(target.transform.position - transform.position);
    }

    // Whatever direction you're walking, but not up or down.
    public virtual void PointInDirectionXZ(Vector3 direction)
    {
        transform.forward = new Vector3(direction.x, 0f, direction.z).normalized;
    }

    public virtual void KnockBack(Vector3 source, float amount)
    {
        Vector3 force = (transform.position - source).normalized * amount;
        rb.AddForce(force, ForceMode.Impulse);

    }

    void EnforceMaxSpeed()
    {
        Vector3 flatSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Your speed in only horizontal directions
        Vector3 boundedSpeed = Vector3.ClampMagnitude(flatSpeed, maxSpeed);
        rb.velocity = new Vector3(boundedSpeed.x, rb.velocity.y, boundedSpeed.z);   // Keep the old y speed, but use the clamped x and z
    }
}
