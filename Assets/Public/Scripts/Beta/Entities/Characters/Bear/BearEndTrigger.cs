using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BearController>())
        {
            other.GetComponent<BearController>().ReachEnd();
        }
    }
}
