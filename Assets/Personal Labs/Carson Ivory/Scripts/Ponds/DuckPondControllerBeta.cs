using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPondControllerBeta : PondControllerBeta
{
    public StructureControllerBeta structure;   // For now, just the main turret

    public void Update()
    {
        if(spawner == null)
        {
            if(GameManagerBeta.Instance != null)
            {
                Instantiate(GameManagerBeta.Instance.styPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
