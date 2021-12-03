using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControllerBeta : MonoBehaviour
{
    public float lifetime;
    protected float age;
    public float cleanupDistance;

    public virtual void Start()
    {
        age = 0f;
    }

    public virtual void Update()
    {
        age += Time.deltaTime;
        if(age >= lifetime || transform.position.y < 0f || transform.position.magnitude > cleanupDistance)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
