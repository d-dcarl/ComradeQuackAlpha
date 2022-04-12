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
            start = true;
            end = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void goToLevel()
    {
        levelsChange = true;
        end = true;
    }

    public void switchToLevelSelect()
    {
        Level_Select.active = true;
        Main_Menu.active = false;
        Title_Screen.active = false;
        Ukraine_Message.active = false;
        Settings.active = false;
        Credits.active = false;
    }

    public void goToTitle()
    {
        titleChange = true;
        end = true;
    }
    public void switchToTitle()
    {
        Title_Screen.active = true;
        Main_Menu.active = false;
        Ukraine_Message.active = false;
        Level_Select.active = false;
        Settings.active = false;
        Credits.active = false;
    }

    public void goToMain()
    {
        mainChange = true;
        end = true;
    }

    public void switchToMain()
    {
        Main_Menu.active = true;
        Ukraine_Message.active = false;
        Title_Screen.active = false;
        Level_Select.active = false;
        Settings.active = false;
        Credits.active = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
