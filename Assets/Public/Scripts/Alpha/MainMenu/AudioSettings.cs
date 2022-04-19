using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private FMOD.Studio.VCA vcacontroller;
    public string vcaName;
    //private FMOD.Studio.VCA Music;
    //private FMOD.Studio.VCA SFX;
   // private FMOD.Studio.VCA UI;

    private Slider slider;
    void Start()
    {
        vcacontroller = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        //Music = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        //SFX = FMODUnity.RuntimeManager.GetVCA("vca:/gameeplay_sfx");
        //UI = FMODUnity.RuntimeManager.GetVCA("vca:/ui_sfx");
        slider = GetComponent<Slider>();
    }

    public void SetVolume(float volume)
    {
        vcacontroller.setVolume(volume);
    }
}
