using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoints : MonoBehaviour
{
    public GameObject player;
    public GameObject bear;
    void OnTriggerEnter(Collider other)
    {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            player.GetComponent<PlayerMovement>().enterDialogue = true;
            player.GetComponent<PlayerMovement>().isMounted = false;
            player.GetComponent<PlayerMovement>().respawnPosition.position = player.GetComponent<Transform>().position;
            gameObject.GetComponent<DialogueInteractable>().TriggerDialogue();
            Destroy(gameObject.GetComponent<BoxCollider>());
            
            
    }
}

