using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject small;
    public GameObject full;

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
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
