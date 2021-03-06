using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControllerBeta : StructureControllerBeta
{
    public float spawnRadius;
    public float spawnHeight;
    protected List<GameObject> spawned;

    public override void Start()
    {
        base.Start();
        spawned = new List<GameObject>();
    }

    public override void Update()
    {
        base.Update();
        CleanSpawnedList();
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

    public virtual GameObject Spawn(GameObject prefab)
    {
        Vector3 offset = Random.onUnitSphere;                       // Random direction
        offset = new Vector3(offset.x, 0f, offset.z).normalized;    // Flatten and make the offset 1 unit long
        Vector3 spawnPosition = transform.position + (offset * spawnRadius) + (Vector3.up * spawnHeight);       // Make sure they don't spawn in the ground
        GameObject newObject = Instantiate(prefab, spawnPosition, transform.rotation);
        spawned.Add(newObject);
        return newObject;
    }

    // Use to make ducks remove themselves from the list when they die
    public void RemoveObject(GameObject toRemove)
    {
        if(spawned.Contains(toRemove))
        {
            spawned.Remove(toRemove);
        }
    }
}
