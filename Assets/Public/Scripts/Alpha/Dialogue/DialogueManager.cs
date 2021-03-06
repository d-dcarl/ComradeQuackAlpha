using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;
    //public GameObject manager;

    private Queue<string> sentences;
    bool isConversationActive = false;

    void Start()
    {
        sentences = new Queue<string>();

    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isConversationActive)
                DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        //manager.GetComponent<AudioSource>().Play();
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        isConversationActive = true;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/characters/dialogue/player", GetComponent<Transform>().position);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (sentence.Contains(">"))
        {
            FindObjectOfType<TutorialManager>().ChatUpdateCommand(sentence);
            DisplayNextSentence();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        

    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (letter == '.' || letter == '!' || letter == '?' || letter == ',')
                yield return new WaitForSeconds(0.1f);
            else
                yield return new WaitForSeconds(0.005f);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of Conversation");
        animator.SetBool("IsOpen", false);
        FindObjectOfType<TutorialManager>().EndDialogue();
        isConversationActive = false;
    }

}
