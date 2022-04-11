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
    [SerializeField] private bool anyKey = true;
    [SerializeField] private bool mainChange = false;

    public void Start()
    {
        StartCoroutine(FadeInTime());
        
    }

    public void Update()
    {
        if(start)
        {
            StartCoroutine(FadeInTime());
            FadeIn();
        }

        if (end)
        {
            StartCoroutine(FadeOutTime());
            FadeOut();
        }

        if (anyKey)
        {
            if(Input.anyKey)
            {
                end = true;
                mainChange = true;
                anyKey = false;
            }
        }

    }

    IEnumerator FadeInTime()
    {
        yield return new WaitForSeconds(3f);
        fadeIn = true;
    }

    public void FadeIn()
    {
        if (fadeIn)
        {
            Fade_Alpha.alpha -= Time.deltaTime;
            if (Fade_Alpha.alpha == 0)
            {
                fadeIn = false;
                start = false;
            }
        }
    }

    IEnumerator FadeOutTime()
    {
        yield return new WaitForSeconds(3f);
        fadeOut = true;
    }

    public void FadeOut()
    {
        if (fadeOut)
        {
            Fade_Alpha.alpha += Time.deltaTime;
            if (Fade_Alpha.alpha == 1)
            {
                if (mainChange == true)
                {
                    switchToMain();
                    mainChange = false;
                }
                fadeOut = false;
                start = true;
                end = false;
            }
        }
    }

    public void switchToMain()
    {
        Main_Menu.active = true;
        Ukraine_Message.active = false;
        //Level_Select.active = false;
        //Settings.active = false;
        //Credits.active = false;
    }








    /*  public GameObject creditsImage;
      public GameObject videoPlayer;
      public AudioSource backgroundMusic;
      public GameObject levelSelect;
      public GameObject[] mainMenu;

      public void Start()
      {
          Cursor.lockState = CursorLockMode.None;
          Cursor.visible = true;
      }

      private void Update()
      {
          //if (!videoPlayer.activeSelf)
          //{
          //    Debug.Log("no mute");
          //    backgroundMusic.mute = false;
          //}
          if (Input.GetKeyDown(KeyCode.D))
          {
              SceneManager.LoadScene("DressingRoom");
          }
      }

      public void PlayGame()
      {
          //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
          levelSelect.SetActive(true);
          foreach (GameObject uiElement in mainMenu)
          {
              uiElement.SetActive(false);
          }
      }

      public void FullScreen(bool isFull)
      {

          Screen.fullScreen = isFull;
          if (isFull)
          {
              small.SetActive(true);
              full.SetActive(false);
          }
          if(!isFull)
          {
              small.SetActive(false);
              full.SetActive(true);
          }


      }

      public void PlayCredits()
      {
          videoPlayer.SetActive(true);
          creditsImage.SetActive(true);
          //backgroundMusic.mute = true;
      }

      public void ExitGame()
      {
          Application.Quit();
      }*/

}
