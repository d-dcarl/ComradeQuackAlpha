using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Waves/Wave")]
public class WaveObject : ScriptableObject
{
    public EnemyWave[] enemyWaves;

    [HideInInspector]
    public int currentWave = 0;

    public GameObject GetCurrentEnemy(float time)
    {
        foreach (EnemyWave ew in enemyWaves)
        {
            if (time >= ew.spawnTimeSinceStart && !ew.spawned)
            {
                return ew.enemy;
            }
        }
        return enemyWaves[0].enemy;
        //if (time >= enemyWaves[currentWave].spawnTimeSinceStart)
        //    return enemyWaves[currentWave].enemy;
        //return enemyWaves[0].enemy;
    }

    public void SetIfSpawned(int count)
    {
        if (count >= enemyWaves[currentWave].amountToSpawn)
        {
            enemyWaves[currentWave].spawned = true;
            currentWave++;
        }
    }
}

[System.Serializable]
public class EnemyWave
{
    //Pig prefab
    //Amount in wave
    //Time since beginning
    [Tooltip("The enemy prefab you want to spawn")]
    public GameObject enemy;
    [Tooltip("The amount of enemies you want to spawn")]
    public int amountToSpawn;
    [Tooltip("The amount of time from the beginning of the wave (in seconds)")]
    public float spawnTimeSinceStart;
    [HideInInspector]
    public bool spawned;
}