using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialTriggers;
    int activeList = 0;
    public GameObject[] cameras; // 1 = start overlook, 2 = rock look, 3 = pond overlook, 4 = pigsty look, 5 = pond zoom
    int mainCamIndex = 0;
    public GameObject instructionsPanel;
    public TMP_Text controllsText;

    public GameObject pondPigs;

    public NestControllerBeta nest;
    public TurretControllerBeta nestTurret;

    public GameObject waveManager;
    public bool waveOver;


    [TextArea(3, 10)]
    public string[] instructions;
    //Movement
    //Shooting
    //Recruiting/building turret
    //Defend from wave

    bool allowRecruit = false;


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
        if (activeList == 3 && pondPigs.transform.childCount == 0)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }

        if (activeList == 5 && FindObjectOfType<PlayerControllerBeta>().ducklingsList.Count > 0 && allowRecruit)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }

        //these turret stuff is sooooo hacky lol
        if (activeList == 6 && FindObjectOfType<HitboxControllerBeta>().gameObject.GetComponent<BoxCollider>().enabled)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }

        if (activeList == 7 && FindObjectOfType<PlaceableTurretControllerBeta>().healthBarSlider.value > 0)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }

        if (activeList == 8 && FindObjectOfType<PlaceableTurretControllerBeta>().damage >= 6)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }

        if (activeList == 9 && waveOver)
        {
            tutorialTriggers[activeList].GetComponent<TutorialTriggers>().TriggerEvent();
        }
    }

    public void ReceiveTrigger(string id, TutorialTriggers tt)
    {
        activeList++;
        if (id.Contains("dialogue"))
        {
            tt.keepPlayer = true;
            instructionsPanel.SetActive(false);
        }
        if (id == "dialogue 4")
        {
            nest.turnOffAutoSpawn = false;
            nestTurret.canShoot.Add("Enemy");
            nest.spawnCap = 3;
        }
        if (id == "dialogue 6")
        {
            GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().maxTurrets = 1;
        }
        if (id == "dialogue 9")//wave starts
        {
            GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().maxTurrets = 10;
            //waveManager.SetActive(true);
        }
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
        GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().maxSpeed = 10;
        GameObject.Find("Player Beta").GetComponent<PlayerControllerBeta>().flapSpeed = 15;
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
        else if (command.Contains("inst"))
        {
            instructionsPanel.SetActive(true);
            controllsText.text = instructions[(int)char.GetNumericValue(command[command.Length - 1])];
        }
        else if (command.Contains("recruit"))
        {
            allowRecruit = true;
        }
        else if (command.Contains("wave"))
        {
            waveManager.SetActive(true);
        }
        else if (command.Contains("end"))
        {
            StyControllerBeta.allStys = null;
            SceneManager.LoadScene(2);
        }
    }
}
