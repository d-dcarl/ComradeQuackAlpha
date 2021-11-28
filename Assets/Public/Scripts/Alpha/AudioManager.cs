using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager musicManagerInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(musicManagerInstance == null) {
            musicManagerInstance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
}
