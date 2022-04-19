using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject Ukraine_Message;
    public GameObject Title_Screen;
    public GameObject Main_Menu;
    //public GameObject Outfit_Select;
    public GameObject Level_Select;
    public GameObject Settings;
    public GameObject Credits;
    public GameObject FadeMenu;

    [Header("Fade In & Out Values")]
    [SerializeField] private CanvasGroup Fade_Alpha;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    [Header("Navigation")]
    [SerializeField] private bool start = true;
    [SerializeField] private bool end = false;
    [SerializeField] private bool anyKeys = false;
    [SerializeField] int counter = 0;
    [SerializeField] private bool titleChange = false;
    [SerializeField] private bool mainChange = false;
    [SerializeField] private bool levelsChange = false;
    [SerializeField] private bool settingsChange = false;
    [SerializeField] private bool creditsChange = false;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        if(start)
        {
            StartCoroutine(FadeInTime());
            if (counter == 0)
            {
                StartCoroutine(ButtonDelay());
            }
        }

        if (end)
        {
            StartCoroutine(FadeOutTime());
        }

        if (anyKeys)
        {
            if(Input.anyKeyDown)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/button_click", GetComponent<Transform>().position);
                end = true;
                counter++; 
                anyKeys = false;
                if (counter == 1)
                {
                    titleChange = true;
                    StartCoroutine(ButtonDelay());
                }
                else if (counter == 2)
                {
                   mainChange = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/button_click", GetComponent<Transform>().position);
            SceneManager.LoadScene("DressingRoom");
        }

    }

    IEnumerator ButtonDelay()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        anyKeys = true;
    }

    IEnumerator FadeInTime()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FadeIn();
    }

    public void FadeIn()
    {
        Fade_Alpha.alpha -= Time.deltaTime;
        if (Fade_Alpha.alpha <= 0)
        {
            start = false; 
        }
    }

    IEnumerator FadeOutTime()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FadeOut();
    }

    public void FadeOut()
    {
        Fade_Alpha.alpha += Time.deltaTime;
        if (Fade_Alpha.alpha >= 1)
        {
            if (mainChange == true)
            {
                switchToMain();
                mainChange = false;
            }
            if (titleChange == true)
            {
                switchToTitle();
                titleChange = false;
            }
            if (levelsChange == true)
            {
                switchToLevelSelect();
                levelsChange = false;
            }
            if (settingsChange == true)
            {
                switchToSettings();
                settingsChange = false;
            }
            if (creditsChange == true)
            {
                switchToCredits();
                creditsChange = false;
            }
            start = true;
            end = false;
        }
    }

    public void PlayGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/start_game", GetComponent<Transform>().position);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void goToLevel()
    {
        levelsChange = true;
        end = true;
    }

    public void switchToLevelSelect()
    {
        Level_Select.SetActive(true);
        Main_Menu.SetActive(false);
        Title_Screen.SetActive(false);
        Ukraine_Message.SetActive(false);
        Settings.SetActive(false);
        Credits.SetActive(false);
    }

    public void goToTitle()
    {
        titleChange = true;
        end = true;
    }

    public void switchToTitle()
    {
        Title_Screen.SetActive(true);
        Main_Menu.SetActive(false);
        Ukraine_Message.SetActive(false);
        Level_Select.SetActive(false);
        Settings.SetActive(false);
        Credits.SetActive(false);
    }

    public void goToMain()
    {
        mainChange = true;
        end = true;
    }

    public void switchToMain()
    {
        Main_Menu.SetActive(true);
        Ukraine_Message.SetActive(false);
        Title_Screen.SetActive(false);
        Level_Select.SetActive(false);
        Settings.SetActive(false);
        Credits.SetActive(false);
    }

    public void goToSettings()
    {
        settingsChange = true;
        end = true;
    }

    public void switchToSettings()
    {
        Settings.SetActive(true);
        Main_Menu.SetActive(false);
        Ukraine_Message.SetActive(false);
        Title_Screen.SetActive(false);
        Level_Select.SetActive(false);
        Credits.SetActive(false);
    }

    public void goToCredits()
    {
        creditsChange = true;
        end = true;
    }

    public void switchToCredits()
    {
        Credits.SetActive(true);
        Settings.SetActive(false);
        Main_Menu.SetActive(false);
        Ukraine_Message.SetActive(false);
        Title_Screen.SetActive(false);
        Level_Select.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
