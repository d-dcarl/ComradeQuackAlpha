using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerBeta : ProjectileControllerBeta
{
    public float speed;
    public int damage;
    public float knockback;

    [HideInInspector]
    public Vector3 direction;

    public List<string> canHit;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        transform.Translate(direction * Time.deltaTime * speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(canHit.Contains(other.tag))
        {
            EntityControllerBeta ecb = other.gameObject.GetComponent<EntityControllerBeta>();
            if(ecb != null)
            {
                ecb.TakeDamage(damage);
            }
            CharacterControllerBeta ccb = other.gameObject.GetComponent<CharacterControllerBeta>();
            if(ccb != null)
            {
                ccb.KnockBack(transform.position, knockback);
            }
            Destroy(gameObject);
        }   
    }
}
