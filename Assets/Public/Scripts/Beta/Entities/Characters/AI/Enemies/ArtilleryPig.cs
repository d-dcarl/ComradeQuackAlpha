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
        GameObject proj = Instantiate(projectile, cannon.position, cannon.rotation, GameObject.Find("Ball Holder").transform);
        proj.GetComponent<Rigidbody>().AddForce(cannon.forward * xForce);
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
