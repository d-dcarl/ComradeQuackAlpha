using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialTriggers;
    int activeList = 0;
    public GameObject[] cameras; // 1 = start overlook, 2 = rock look, 3 = pond overlook, 4 = pigsty look, 5 = pond zoom
    int mainCamIndex = 0;
    public GameObject instructionsPanel;
    public TMP_Text controllsText;
    [TextArea(3, 10)]
    public string[] instructions;

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
            instructionsPanel.SetActive(false);
        }
        //if (id == "dialogue 1")
        //{
        //    cameras[mainCamIndex].SetActive(false);
        //    cameras[1].SetActive(true);
        //}
        //if (id == "dialogue 2")
        //{
        //    cameras[mainCamIndex].SetActive(false);
        //    cameras[2].SetActive(true);
        //}
        //if (id == "dialogue 3")
        //{
        //    cameras[mainCamIndex].SetActive(false);
        //    cameras[3].SetActive(true);
        //}
    }

    public void EndDialogue()
    {
        foreach(TutorialTriggers tt in FindObjectsOfType<TutorialTriggers>())
        {
            tt.keepPlayer = false;
        }
        foreach (GameObject go in cameras)
        {
            go.SetActive(false);
        }
        cameras[mainCamIndex].SetActive(true);
        tutorialTriggers[activeList - 1].SetActive(false);
        if (activeList < tutorialTriggers.Length)
            tutorialTriggers[activeList].SetActive(true);
    }

    public void ChatUpdateCommand(string command)
    {
        if (command.Contains("cam"))
        {
            foreach (GameObject go in cameras)
            {
                go.SetActive(false);
            }

            cameras[(int)char.GetNumericValue(command[command.Length - 1])].SetActive(true);
        }
    }
}
