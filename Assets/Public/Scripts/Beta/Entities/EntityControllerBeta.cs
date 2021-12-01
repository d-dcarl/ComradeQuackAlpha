using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControllerBeta : MonoBehaviour
{
    public int maxHealth;

    protected int currentHealth;
    [HideInInspector]
    public bool alive;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        alive = true;
    }

    public virtual void Update()
    {
        if (currentHealth <= 0 || transform.position.y < 0f)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        alive = false;
        Destroy(gameObject);
    }
}
