using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControllerBeta : StructureControllerBeta
{
    public GameObject spawnPrefab;  // Eventually this could be a list
    public int spawnCap;
    public float spawnDelay;
    protected float spawnTimer;
    public float spawnRadius;
    public float spawnHeight;

    protected List<GameObject> spawned;

    public override void Start()
    {
        base.Start();
        spawnTimer = spawnDelay;
        spawned = new List<GameObject>();
    }

    public override void Update()
    {
        base.Update();
        CleanSpawnedList();
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && spawned.Count < spawnCap)
        {
            Spawn();
            spawnTimer = spawnDelay;
        }
    }

    public void CleanSpawnedList()
    {
        int i = 0;
        while (i < spawned.Count)
        {
            if(spawned[i] == null)
            {
                spawned.RemoveAt(i);
            } else
            {
                i++;
            }
        }
    }

    public void Spawn()
    {
        Vector3 offset = Random.onUnitSphere;                       // Random direction
        offset = new Vector3(offset.x, 0f, offset.z).normalized;    // Flatten and make the offset 1 unit long
        Vector3 spawnPosition = transform.position + (offset * spawnRadius) + (Vector3.up * spawnHeight);       // Make sure they don't spawn in the ground
        GameObject newObject = Instantiate(spawnPrefab, spawnPosition, transform.rotation);
        spawned.Add(newObject);
    } 
}
