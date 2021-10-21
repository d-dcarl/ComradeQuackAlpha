using UnityEngine;

public class Target : MonoBehaviour
{
    // Reference: https://youtu.be/THnivyG0Mvo

    public float health = 30f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
