using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameManager gm;
    public WaveObject[] wavesToSpawn;
    public GameObject mainPond;
    public GameObject mainPigsty;
    public GameObject pigParent;
    public float spawnRate;
    public float timeBetweenWaves;
    public int currentWave;
    public bool isWaitActive;

    float spawnTimer = 0;
    float waveTime = 0;
    float waitTime = 0;
    int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaitActive)
        {
            SpawnWave();
        }
        if (isWaitActive)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= timeBetweenWaves)
            {
                waitTime = 0;
                currentWave++;
                isWaitActive = false;
            }
        }
    }

    private void SpawnWave()
    {
        //Checks to make sure there are waves to spawn
        if (currentWave < wavesToSpawn.Length)
        {
            waveTime += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate)
            {
                mainPigsty.GetComponent<PondController>().SpawnPig(wavesToSpawn[currentWave].GetCurrentEnemy(waveTime), pigParent);
                spawnTimer = 0;
                spawnCount++;
            }
        }
    }
}
