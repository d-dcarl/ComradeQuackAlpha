using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyControllerBeta : PondControllerBeta
{
    public void Update()
    {
        if (spawner == null)
        {
            if (GameManagerBeta.Instance != null)
            {
                Instantiate(GameManagerBeta.Instance.duckPondPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
