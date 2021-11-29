using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControllerBeta : MonoBehaviour
{
    public int maxHealth;

    protected int currentHealth;

    public virtual void Start()
    {
        currentHealth = maxHealth;
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
        Destroy(gameObject);
    }
}
