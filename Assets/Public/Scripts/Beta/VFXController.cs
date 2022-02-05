using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public ParticleSystem[] emitters;
    public GameObject[] objectsToSpawn;
    [Tooltip("It will pick a random number between 0 and this to spawn a random object to spawn")]
    public float maxWaitTime;
    float waitTimeCounter = 0;
    float currentWaitTime = 0;

    float ConeSize;

    bool isEmitting;

    // Start is called before the first frame update
    void Start()
    {
        isEmitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartVFX();
        }
        if (isEmitting)
        {
            waitTimeCounter += Time.deltaTime;
            if (waitTimeCounter >= currentWaitTime)
            {
                if (objectsToSpawn.Length > 0)
                {
                    float xSpread = Random.Range(-1, 1);
                    float ySpread = Random.Range(-1, 1);
                    //normalize the spread vector to keep it conical
                    Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * ConeSize;
                    Quaternion rotation = Quaternion.Euler(spread) * transform.rotation;
                    Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)], transform.position, rotation);
                }
                currentWaitTime = Random.Range(0f, maxWaitTime);
                waitTimeCounter = 0;
            }
        }
    }

    public void StartVFX()
    {
        foreach(ParticleSystem ps in emitters)
        {
            ps.Play();
        }
        isEmitting = true;
        currentWaitTime = Random.Range(0f, maxWaitTime);
        waitTimeCounter = 0;
    }

    public void StopVFX()
    {
        foreach (ParticleSystem ps in emitters)
        {
            ps.Stop();
        }
        isEmitting = false;
    }
}
