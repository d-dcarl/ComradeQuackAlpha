using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEditor;

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

    private MenuItem currentMenuItem = MenuItem.UkraineMessage;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        slider = GetComponent<Slider>();

        StartCoroutine(StartFadeIn());
    }

    public void Update()
    {
        if (anyKeys && Input.anyKeyDown)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/button_click", GetComponent<Transform>().position);
            anyKeys = false;
            
            if (currentMenuItem == MenuItem.UkraineMessage)
                StartCoroutine(FadeOut(MenuItem.TitleScreen));
            else if (currentMenuItem == MenuItem.TitleScreen)
                StartCoroutine(FadeOut(MenuItem.MainMenu));
        }

    }

    private IEnumerator StartFadeIn()
    {
        anyKeys = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Fade_Alpha.alpha -= 0.1f;
        }

        Fade_Alpha.alpha = 0f;

        anyKeys = true;
    }

    private IEnumerator FadeOut(MenuItem fadeTo)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Fade_Alpha.alpha += 0.1f;
        }

        Fade_Alpha.alpha = 1f;

        switch (fadeTo)
        {
            case MenuItem.MainMenu:
                switchToMain();
                break;
            case MenuItem.TitleScreen:
                switchToTitle();
                break;
            case MenuItem.LevelSelect:
                switchToLevelSelect();
                break;
            case MenuItem.Settings:
                switchToSettings();
                break;
            case MenuItem.Credits:
                switchToCredits();
                break;
            default:
                Debug.Log("Invalid menu item, this should never happen");
                break;
        }

        currentMenuItem = fadeTo;

        StartCoroutine(StartFadeIn());
    }

    public void PlayGame()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/start_game", GetComponent<Transform>().position);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void goToLevel()
    {
        StartCoroutine(FadeOut(MenuItem.LevelSelect));
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
        StartCoroutine(FadeOut(MenuItem.MainMenu));
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
        StartCoroutine(FadeOut(MenuItem.Settings));
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
        StartCoroutine(FadeOut(MenuItem.Credits));
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

    public void goToDressingRoom()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ui/main/button_click", GetComponent<Transform>().position);
        SceneManager.LoadScene("DressingRoom");
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

    private enum MenuItem
    {
        Credits,
        Settings,
        MainMenu,
        UkraineMessage,
        TitleScreen,
        LevelSelect
    }

}
