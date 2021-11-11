using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> tracked;

    private void Start()
    {
        tracked = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!tracked.Contains(other.gameObject))
        {
            tracked.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        tracked.Remove(other.gameObject);
    }
}
