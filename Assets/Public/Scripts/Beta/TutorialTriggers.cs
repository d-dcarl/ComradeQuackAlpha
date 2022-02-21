using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    public string ID;
    public bool keepPlayer;//Used to keep the player in place for dialogue parts

    private void Update()
    {
        if (keepPlayer)
        {
            GameObject.Find("Player Beta").transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<TutorialManager>().ReceiveTrigger(ID, this);
            if (GetComponent<DialogueInteractable>())
                GetComponent<DialogueInteractable>().TriggerDialogue();
        }
    }
}
