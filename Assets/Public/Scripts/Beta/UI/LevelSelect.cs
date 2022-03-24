using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject[] mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Back()
    {
        foreach (GameObject uiElement in mainMenu)
        {
            uiElement.SetActive(true);
        }
        
        gameObject.SetActive(false);
    }
}
