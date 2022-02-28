using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigControllerBeta : EnemyControllerBeta
{
    [SerializeField] public bool isGrounded;
    //was 30f, reduced to 10f
    public float jumpSpeed = 10f;
    protected float jumpTimer;
    public float lowerJumpDelay = 2f;
    public float upperJumpDelay = 10f;

    public override void Start()
    {
        base.Start();
        jumpTimer = Random.Range(lowerJumpDelay, upperJumpDelay);
    }

    public override void Update()
    {
        base.Update();
        if(!isGrounded)
        {
            if (jumpTimer <= 0)
            {
                pigJump();
            }
            else
            {
                jumpTimer -= Time.deltaTime;
            }
        }

    }

    public void pigJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        jumpTimer = Random.Range(lowerJumpDelay, upperJumpDelay);
    }
}
