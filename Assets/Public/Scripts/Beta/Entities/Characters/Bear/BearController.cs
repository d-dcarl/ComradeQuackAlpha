using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    public GameObject[] targets;
    private int currentTargetIndex = 0;
    public float speed;
    public float damageDelt;
    public float knockBack;
    public float attackTimeMax;

    public bool go;

    private Animator animator;
    private float attackTimerCount;
    private bool canStomp = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        attackTimerCount = attackTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            transform.LookAt(targets[currentTargetIndex].transform);
            transform.position += transform.forward * speed * Time.deltaTime;
            
            if (!animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", true);
                animator.Play("Bear_Walk");
                Debug.Log("walk");
            }
            attackTimerCount -= Time.deltaTime;
            if (attackTimerCount <= 0)
            {
                canStomp = true;
            }
            
        }
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bear Stomp") && !animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        //{
        //    go = true;
        //    animator.Play("Bear_Walk");
        //    animator.SetBool("isAttacking", false);
        //    Debug.Log("Stop stomp and go");
        //}
    }

    public void Attack(GameObject enemy)
    {
        enemy.GetComponent<EnemyControllerBeta>().TakeDamage(damageDelt);
        enemy.GetComponent<Rigidbody>().AddForce(transform.forward * knockBack);
        if (canStomp)
        {
            canStomp = false;
            attackTimerCount = attackTimeMax;
            go = false;
            animator.SetBool("isAttacking", true);
            animator.Play("Bear Stomp");
            Debug.Log("Stop moving and stomp");
            StartCoroutine(BackToWalk());
        }
    }

    public void ReachNode()
    {
        currentTargetIndex++;
    }

    public void ReachEnd()
    {
        go = false;
        gameObject.SetActive(false);
    }

    IEnumerator BackToWalk()
    {
        yield return new WaitForSeconds(3f);
        go = true;
        animator.Play("Bear_Walk");
        animator.SetBool("isAttacking", false);
        Debug.Log("Stop stomp and go");
    }

}
