using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int currentScene;
    public int maxScenes;
    void Awake()
    {
        if (Object.FindObjectsOfType<SceneTransition>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //previous scene
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            currentScene--;
            if (currentScene < 0)
                currentScene = maxScenes - 1;
            Debug.Log(currentScene);
            SceneManager.LoadScene(currentScene);
        }
        //next scene
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            currentScene++;
            if (currentScene >= maxScenes)
                currentScene = 0;
            Debug.Log(currentScene);
            SceneManager.LoadScene(currentScene);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
