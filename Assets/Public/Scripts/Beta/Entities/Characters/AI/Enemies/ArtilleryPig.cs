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
    public float zForce;
    public Transform cannon;
    public Transform targetBox;

    bool canMove = true;
    bool isProgressing = false;
    bool isFiring = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        transform.LookAt(targetBox);
        GameObject proj = Instantiate(projectile, cannon.position, cannon.rotation, GameObject.Find("Ball Holder").transform);
        float dist = Vector3.Distance(cannon.position, targetBox.position);
        float t = .5f;
        float xVel = -(cannon.position.x - targetBox.position.x) / t;
        float yVel = ((cannon.position.y - targetBox.position.y) / t) + ((Physics.gravity.y * t)/2);
        float zVel = -(cannon.position.z - targetBox.position.z) / t;
        //proj.GetComponent<Rigidbody>().velocity = new Vector3(xVel, yVel, zVel);
        proj.GetComponent<Rigidbody>().velocity = cannon.forward * (dist * 2);
        foreach (GameObject g in attackHitBox.tracked)
        {
            if (g != null)
            {
                // Maybe implement a CanDamage list at some point
                if (canAttack.Contains(g.tag))
                {
                    //EntityControllerBeta ecb = g.GetComponent<EntityControllerBeta>();
                    //ecb.TakeDamage(attackDamage);
                }
            }
        }
    }
}
