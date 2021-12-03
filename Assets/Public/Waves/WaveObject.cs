using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Waves/Wave")]
public class WaveObject : ScriptableObject
{
    [Tooltip("The index of which pigsty you want to spawn from, 0 is first")]
    public int pigstyToSpawn;
    public EnemyWave[] enemyWaves;
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
}