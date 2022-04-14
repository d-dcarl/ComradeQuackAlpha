using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public EventReference reference = new EventReference();
    [EventRef(MigrateTo = "reference"), SerializeField] string audioName = default;
    FMOD.Studio.EventInstance audioEvent;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySoundEvent()
    {
        audioEvent = RuntimeManager.CreateInstance(audioName);
        audioEvent.start();
    }


}
