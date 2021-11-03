using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public GameObject player;
    public bool trigger = false;
    void OnTriggerEnter(Collider other)
    {
            player.GetComponent<PlayerMovement>().respawnPosition.position = player.GetComponent<Transform>().position;
            gameObject.GetComponent<DialogueInteractable>().TriggerDialogue();
            Destroy(gameObject.GetComponent<BoxCollider>());
            //Destroy(GetComponent<DialogueInteractable>());
            
    }
}

