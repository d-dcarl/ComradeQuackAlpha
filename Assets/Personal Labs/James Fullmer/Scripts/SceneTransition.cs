using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int currentScene;
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
                currentScene = SceneManager.sceneCount - 1;
            SceneManager.LoadScene(currentScene);
        }
        //next scene
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            currentScene++;
            if (currentScene >= SceneManager.sceneCount)
                currentScene = 0;
            SceneManager.LoadScene(currentScene);
        }
    }
}
