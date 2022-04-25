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

    private Animator animator;

    public override void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/characters/enemies/basic_pig/spawn", GetComponent<Transform>().position);
        animator = GetComponentInChildren<Animator>();
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

    public override void Attack()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/characters/enemies/basic_pig/melee_attack", GetComponent<Transform>().position);
        animator.SetTrigger("PigAttack");
        animator.Play("PigAttack");
        base.Attack();
    }

    public override void Die()
    {
        animator.SetTrigger("PigDeath");
        animator.Play("PigDeath");
        base.Die();
    }

    public override void TakeDamage(float amount)
    {
        animator.SetTrigger("PigHurt");
        animator.Play("PigHurt");
        base.TakeDamage(amount);
    }
}
