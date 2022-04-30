using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    public VideoPlayer player;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!player.isPlaying && timer >= 1)
        {
            StyControllerBeta.allStys = null;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
