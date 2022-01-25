using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryShell : BulletControllerBeta
{
    public Transform target;
    public float fallSpeed;

    Rigidbody rb;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //base.Update();
        age += Time.deltaTime;
        if (age >= lifetime || transform.position.y < 0f || transform.position.magnitude > cleanupDistance)
        {
            Die();
        }
        if (rb.velocity.y < 0)
        {
            Move();
        }
    }

    protected override void Move()
    {
        if (target)
        {
            transform.LookAt(target);
            rb.velocity = transform.forward * fallSpeed;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("I destroyed");
    }
}
