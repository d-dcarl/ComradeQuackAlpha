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
            //GameObject.Find("Player Beta").transform.position = transform.position;
            GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().maxSpeed = 0;
            GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().flapSpeed = 0;
            GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().inDialogue = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TriggerEvent();
        }
    }

    public void TriggerEvent()
    {
        FindObjectOfType<TutorialManager>().ReceiveTrigger(ID, this);
        if (GetComponent<DialogueInteractable>())
            GetComponent<DialogueInteractable>().TriggerDialogue();
    }
}
