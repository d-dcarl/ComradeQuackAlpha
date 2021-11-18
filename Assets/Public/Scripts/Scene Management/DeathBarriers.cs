using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarriers : MonoBehaviour
{
    public GameObject player;

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().playerDeath();
            //other.gameObject.transform.position = ;
        }
    }
}

