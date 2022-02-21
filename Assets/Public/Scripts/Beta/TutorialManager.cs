using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialTriggers;
    int activeList = 0;

    //Movement
    //Shooting
    //Recruiting/building turret
    //Defend from wave


    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in tutorialTriggers)
        {
            go.SetActive(false);
        }
        tutorialTriggers[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveTrigger(string id, TutorialTriggers tt)
    {
        activeList++;
        if (id.Contains("dialogue"))
        {
            tt.keepPlayer = true;
        }
    }

    public void EndDialogue()
    {
        foreach(TutorialTriggers tt in FindObjectsOfType<TutorialTriggers>())
        {
            tt.keepPlayer = false;
        }
        tutorialTriggers[activeList - 1].SetActive(false);
        if (activeList < tutorialTriggers.Length)
            tutorialTriggers[activeList].SetActive(true);
    }
}
