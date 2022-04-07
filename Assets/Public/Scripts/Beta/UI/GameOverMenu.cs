using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverMenu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject mainMenuButton;
    public VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!video.isPlaying)
        {
            continueButton.SetActive(true);
            mainMenuButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
            mainMenuButton.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToGame()
    {
        StyControllerBeta.allStys = null;
        SceneManager.LoadScene("Level 1");
    }

    public void ReturnToMainMenu()
    {
        //Change later to main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
