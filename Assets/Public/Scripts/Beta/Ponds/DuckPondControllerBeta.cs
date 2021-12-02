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
                PondControllerBeta newPCB = Instantiate(GameManagerBeta.Instance.styPrefab, transform.position, transform.rotation).GetComponent<PondControllerBeta>();
                newPCB.neighbors = new List<PondControllerBeta>();

                foreach(PondControllerBeta neighbor in neighbors)
                {
                    neighbor.neighbors.Remove(this);

                    neighbor.neighbors.Add(newPCB);
                    newPCB.neighbors.Add(neighbor);
                }
                
                foreach(StyControllerBeta scb in GameManagerBeta.Instance.allStys)
                {
                    scb.ResetTarget();
                }
                Destroy(gameObject);
            }
        }
    }
}
