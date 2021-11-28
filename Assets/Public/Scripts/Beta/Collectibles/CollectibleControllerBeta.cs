using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleControllerBeta : MonoBehaviour
{
    public int resourceType;

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerControllerBeta pcb = other.GetComponent<PlayerControllerBeta>();
        if(pcb != null)
        {
            pcb.CollectResource(resourceType, 1);
            Destroy(gameObject);
        }
    }
}
