using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public particleSetting setting;

    [Header("High Settings")]
    public float high_lifetime;
    public int high_rate;

    [Header("Low Settings")]
    public float low_lifetime;
    public int low_rate;

    ParticleSystem partSys;
    // Start is called before the first frame update
    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        var main = partSys.main;
        var emission = partSys.emission;

        if (setting == particleSetting.High)
        {
            main.startLifetime = high_lifetime;
            emission.rateOverDistance = high_rate;
        }
        if (setting == particleSetting.Low)
        {
            main.startLifetime = low_lifetime;
            emission.rateOverDistance = low_rate;
        }
        if (setting == particleSetting.Off)
        {
            gameObject.SetActive(false);
        }
    }

    public enum particleSetting
    {
        High,
        Low,
        Off
    }
}
