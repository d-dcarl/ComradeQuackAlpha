using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigstyController : MonoBehaviour
{
    public GameObject pigPrefab;

    public float spawnDelay;
    private float spawnTimer;

    public float offsetAmount;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            spawnTimer = spawnDelay;
            Debug.Log("Spawn Pig!");
            Vector3 randomOffset = Random.onUnitSphere;
            randomOffset.y = 0;
            randomOffset = randomOffset.normalized;
            Instantiate(pigPrefab, transform.position + randomOffset * offsetAmount + Vector3.up, Quaternion.identity);
        }
    }
}
