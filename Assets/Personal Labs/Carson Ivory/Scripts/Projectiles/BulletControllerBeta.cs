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
        if (other.CompareTag("Enemy Structure"))
        {
            Debug.Log("Triggered enemy structure");
        }
        Hit(other.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Structure"))
        {
            Debug.Log("Collided with enemy structure");
        }
        Hit(collision.gameObject);
    }

    public void Hit(GameObject other)
    {
        if (canHit.Contains(other.tag))
        {
            Debug.Log("Hitting: " + other.tag);

            EntityControllerBeta ecb = other.gameObject.GetComponent<EntityControllerBeta>();
            if (ecb != null)
            {
                ecb.TakeDamage(damage);
            }
            CharacterControllerBeta ccb = other.gameObject.GetComponent<CharacterControllerBeta>();
            if (ccb != null)
            {
                ccb.KnockBack(transform.position, knockback);
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Enemy Structure"))
        {
            Debug.Log("Trying to hit enemy structure and failing for some reason");
        }
    }
}
