using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bear")
        {
            Destroy(gameObject);
        }
    }
}
