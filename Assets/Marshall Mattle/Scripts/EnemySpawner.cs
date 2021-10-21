using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject parent;
    public int spawnGroupCount;
    public int spawnAmount = 10;
    public float timeBetweenSpawns = 1.0f;

    float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                for (int j = 0; j < spawnGroupCount; j++)
                {
                    Instantiate(objectToSpawn, new Vector3(this.transform.position.x + Random.Range(-1.0f,1.0f), this.transform.position.y, 
                        this.transform.position.z + Random.Range(-1.0f, 1.0f)), Quaternion.identity, parent.transform);
                }
                spawnTimer = timeBetweenSpawns;
            }
        }
        
    }
}