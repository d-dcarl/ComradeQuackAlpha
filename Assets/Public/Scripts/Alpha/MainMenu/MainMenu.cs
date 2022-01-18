using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject small;
    public GameObject full;
    public GameObject creditsImage;
    public GameObject videoPlayer;
    public AudioSource backgroundMusic;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //private void Update()
    //{
    //    if (!videoPlayer.activeSelf)
    //    {
    //        Debug.Log("no mute");
    //        backgroundMusic.mute = false;
    //    }
    //}

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    }
    
}
