using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyControllerBeta : PondControllerBeta
{
    protected PondControllerBeta bestNeighbor;
    protected float totalDistanceToPond;

    public void Start()
    {
        bestNeighbor = null;
        totalDistanceToPond = -1f;
    }

    public void Update()
    {
        CheckDead();

        UpdateBestNeighbor();
    }

    void CheckDead()
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

    void UpdateBestNeighbor()
    {
        foreach(PondControllerBeta pcb in neighbors)
        {
            if(pcb as DuckPondControllerBeta != null)
            {
                float neighborDist = Vector3.Distance(pcb.transform.position, transform.position);
                if (totalDistanceToPond < 0f || neighborDist < totalDistanceToPond)
                {
                    bestNeighbor = pcb;
                    totalDistanceToPond = neighborDist;
                }
            } else
            {
                StyControllerBeta scb = pcb as StyControllerBeta;
                if (scb != null && scb.totalDistanceToPond > 0f)
                {
                    float neighborTotalDist = Vector3.Distance(scb.transform.position, transform.position) + scb.totalDistanceToPond;
                    if(totalDistanceToPond < 0f || neighborTotalDist < totalDistanceToPond)
                    {
                        bestNeighbor = scb;
                        totalDistanceToPond = neighborTotalDist;
                    }
                }
            }
        }

        if(bestNeighbor != null)
        {
            transform.LookAt(bestNeighbor.transform);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
        }
    }
}
