using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHitbox : MonoBehaviour
{
    public BearController bear;
    public List<GameObject> enemiesInRange;

    private void Start()
    {
        enemiesInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
            bear.Attack(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (enemiesInRange.Contains(other.gameObject))
                enemiesInRange.Remove(other.gameObject);
        }
    }
}
