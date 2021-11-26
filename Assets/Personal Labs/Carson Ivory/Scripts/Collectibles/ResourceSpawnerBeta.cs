using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawnerBeta : MonoBehaviour
{
    public GameObject resourcePrefab;

    public float spawnRadius;
    public float spawnDelay;
    public int spawnCap;
    protected float spawnTimer;

    public virtual void Start()
    {
        spawnTimer = spawnDelay;
    }

    public virtual void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            spawnTimer = spawnDelay;
            SpawnResource();
        }
    }

    public virtual void SpawnResource()
    {
        Vector2 randomVec = Random.insideUnitCircle * spawnRadius;     // Pick a random point inside the spawn radius
        Vector3 spawnPos = transform.position + new Vector3(randomVec.x, 0f, randomVec.y);
        GameObject resource = Instantiate(resourcePrefab, spawnPos, Quaternion.identity);
    }
}
