using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HideVideoPlayer : MonoBehaviour
{
    public GameObject projector;
    VideoPlayer player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying)
        {
            projector.SetActive(false);
        }
        else
            projector.SetActive(true);
    }
}
