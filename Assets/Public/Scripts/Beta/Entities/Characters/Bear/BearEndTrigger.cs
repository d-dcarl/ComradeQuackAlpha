using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEndTrigger : MonoBehaviour
{
    public bool isEnd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BearController>())
        {
            if (isEnd)
                other.GetComponent<BearController>().ReachEnd();
            else
                other.GetComponent<BearController>().ReachNode();
        }
    }
}
