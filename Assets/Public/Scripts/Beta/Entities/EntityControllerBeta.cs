using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityControllerBeta : MonoBehaviour
{
    public float maxHealth;

    protected float currentHealth;
    [HideInInspector]
    public bool alive;
    public Slider healthBarSlider;


    public virtual void Start()
    {
        currentHealth = maxHealth;
        alive = true;
        if (healthBarSlider)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = maxHealth;
        }
    }

    public virtual void Update()
    {
        if (currentHealth <= 0 || transform.position.y < 0f)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthBarSlider)
        {
            healthBarSlider.value = currentHealth;
        }
        if (currentHealth <= 0)
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
