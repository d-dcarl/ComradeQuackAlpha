using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondController : MonoBehaviour
{
    public int maxHealth;
    private int health;

    public GameObject pigstyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.ponds == null)
        {
            GameManager.ponds = new List<PondController>();
        }
        GameManager.ponds.Add(this);

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Converting to pigsty");
        Instantiate(pigstyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
