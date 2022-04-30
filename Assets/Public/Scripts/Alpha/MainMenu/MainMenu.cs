using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

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

    [Header("Display Dropdowns")]
    [SerializeField] public TMP_Dropdown displayType;
    [SerializeField] public TMP_Dropdown resolutionType;

    [Header("Mouse Sensitivity")]
    [SerializeField] public TMP_Text sensitivity;
    [SerializeField] private Slider slider;
    [SerializeField] public float mouseSense;

    private bool toFadeOut;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        slider = GetComponent<Slider>();
        toFadeOut = false;
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

        if (end && !toFadeOut)
        {
            StartCoroutine(FadeOutTime());
        }

        //if (toFadeOut)
        //{
        //    FadeOut();
        //}

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
        //toFadeOut = true;
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
            toFadeOut = false;
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

    public void SetResolutionAndDisplay()
    {
        if (displayType.value == 0 && resolutionType.value == 0)
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow);
        }
        if(displayType.value == 1 && resolutionType.value == 0)
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.MaximizedWindow);
        }
        if (displayType.value == 2 && resolutionType.value == 0)
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.Windowed);
        }
        if (displayType.value == 0 && resolutionType.value == 1)
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
        }
        if (displayType.value == 1 && resolutionType.value == 1)
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.MaximizedWindow);
        }
        if (displayType.value == 2 && resolutionType.value == 1)
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.Windowed);
        }
        if (displayType.value == 0 && resolutionType.value == 2)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        if (displayType.value == 1 && resolutionType.value == 2)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow);
        }
        if (displayType.value == 2 && resolutionType.value == 2)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        }
        if (displayType.value == 0 && resolutionType.value == 3)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
        }
        if (displayType.value == 1 && resolutionType.value == 3)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.MaximizedWindow);
        }
        if (displayType.value ==2 && resolutionType.value == 3)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        if (displayType.value == 0 && resolutionType.value == 4)
        {
            Screen.SetResolution(640, 480, FullScreenMode.FullScreenWindow);
        }
        if (displayType.value == 1 && resolutionType.value == 4)
        {
            Screen.SetResolution(640, 480, FullScreenMode.MaximizedWindow);
        }
        if (displayType.value == 2 && resolutionType.value == 4)
        {
            Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        }
    }

    public void mouseSensitivityText()
    {
        mouseSense = slider.value;
        sensitivity.SetText(mouseSense.ToString());
    }

}
