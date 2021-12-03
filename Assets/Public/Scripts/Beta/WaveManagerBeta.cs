using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerBeta : MonoBehaviour
{
    public WaveObject[] wavesToSpawn;
    //public GameObject mainPond;

    // This needs to work for more than one sty
    public SpawnerControllerBeta[] mainPigsties;
    
    
    public GameObject parentObj;
    public float spawnRate;
    public float timeBetweenWaves;
    public int currentWave;
    public bool isWaitActive;

    float spawnTimer = 0;
    float waveTime = 0;
    float waitTime = 0;
    int spawnCount = 0;
    int waveCount = 0;
    bool canSpawnMore = true;
    int pigCount = 0;

    GameManagerBeta manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManagerBeta>();
        if (!manager)
            Debug.Log("Can't find game manager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaitActive)
        {
            SpawnWave();
            CheckAmountOfPigs();
            CheckWaveOver();
        }
        if (isWaitActive)
        {
            waitTime += Time.deltaTime;
            if (waitTime >= timeBetweenWaves)
            {
                ResetWave();
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
                if (canSpawnMore && waveTime >= wavesToSpawn[currentWave].enemyWaves[waveCount].spawnTimeSinceStart && mainPigsties.Length > 0)
                { 
                    // TODO: Either make all stys spawn an enemy, or choose which sty each enemy spawns at
                    mainPigsties[0].SpawnEnemy(wavesToSpawn[currentWave].enemyWaves[waveCount].enemy, parentObj);
                    spawnTimer = 0;
                    spawnCount++;
                    if (spawnCount >= wavesToSpawn[currentWave].enemyWaves[waveCount].amountToSpawn)
                    {
                        spawnCount = 0;
                        waveCount++;
                        if (waveCount >= wavesToSpawn[currentWave].enemyWaves.Length)
                        {
                            canSpawnMore = false;
                        }
                    }
                }
            }
        }
    }

    private void CheckWaveOver()
    {
        CheckAmountOfPigs();
        if (pigCount == 0 && !canSpawnMore)
        {
            //Wave is over
            isWaitActive = true;
        }
    }

    private void CheckAmountOfPigs()
    {
        pigCount = parentObj.transform.childCount;
    }

    private void ResetWave()
    {
        waitTime = 0;
        currentWave++;
        isWaitActive = false;
        canSpawnMore = true;
        waveCount = 0;
        waveTime = 0;
        if (currentWave >= wavesToSpawn.Length)
        {
            Debug.Log("YOU WIN!");
        }
    }
}
