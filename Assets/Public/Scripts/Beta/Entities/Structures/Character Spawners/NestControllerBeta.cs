using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestControllerBeta : SpawnerControllerBeta
{
    public override GameObject Spawn()
    {
        DucklingControllerBeta newDuckling = base.Spawn().GetComponent<DucklingControllerBeta>();

        if(newDuckling == null)
        {
            Debug.LogError("Nest must spawn ducklings.");
            return null;
        }

        newDuckling.InitializeDuckling(this);
        return newDuckling.gameObject;
    }

    public override void Update()
    {
        base.Update();
        if (!turnOffAutoSpawn)
        {
            CleanSpawnedList();
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && curSpawns < spawnCap)
            {
                Spawn();
                spawnTimer = spawnDelay;
            }
        }
    }
}
