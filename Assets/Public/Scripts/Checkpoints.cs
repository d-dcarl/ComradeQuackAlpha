using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject player;
    void OnTriggerEnter(Collider other)
    {
            player.GetComponent<PlayerMovement>().respawnPosition.position = player.GetComponent<Transform>().position;
            //other.gameObject.transform.position = ;
    }
}

