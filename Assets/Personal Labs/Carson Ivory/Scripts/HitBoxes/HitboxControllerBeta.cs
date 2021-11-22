using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxControllerBeta : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> tracked;

    public virtual void Start()
    {
        tracked = new List<GameObject>();
    }

    public virtual void Update()
    {
        // Remove destroyed objects and any objects of the wrong type from list
        int i = 0;
        while (i < tracked.Count)
        {
            GameObject t = tracked[i];
            if(t == null)
            {
                tracked.Remove(t);
            } else
            {
                i++;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        tracked.Add(other.gameObject);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if(tracked.Contains(other.gameObject)) {
            tracked.Remove(other.gameObject);
        }
    }
}
