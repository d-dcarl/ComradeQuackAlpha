using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControllerBeta : MonoBehaviour
{
    public float lifetime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            Destroy(gameObject);
        }
    }
}
