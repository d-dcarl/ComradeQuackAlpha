using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHitbox : MonoBehaviour
{
    public BearController bear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            bear.Attack(other.gameObject);
        }
    }
}
