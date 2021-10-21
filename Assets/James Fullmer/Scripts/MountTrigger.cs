using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountTrigger : MonoBehaviour
{
    public BearMovement bm;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bm.isMounted = true;
            other.GetComponent<PlayerMovement>().MountDuck(transform, this);
        }
    }

    public void Dismount()
    {
        bm.isMounted = false;
    }
}
