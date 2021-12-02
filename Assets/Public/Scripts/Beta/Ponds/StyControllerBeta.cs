using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyControllerBeta : PondControllerBeta
{
    protected PondControllerBeta bestNeighbor;
    protected float totalDistanceToPond;

    protected bool registered;

    public void Start()
    {
        bestNeighbor = null;
        totalDistanceToPond = -1f;
        registered = false;
        RegisterSty();
    }

    public void Update()
    { 
        if(!registered)
        {
            RegisterSty();
        }
        CheckDead();
        UpdateBestNeighbor();
    }

    

    public PondControllerBeta GetTargetPond()
    {
        return bestNeighbor;
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

    void RegisterSty()
    {
        if(GameManagerBeta.Instance != null)
        {
            if(GameManagerBeta.Instance.allStys == null)
            {
                GameManagerBeta.Instance.allStys = new List<StyControllerBeta>();
            }
            GameManagerBeta.Instance.allStys.Add(this);

            registered = true;
        }
    }

    void CheckDead()
    {
        if (spawner == null)
        {
            if (GameManagerBeta.Instance != null)
            {
                GameManagerBeta.Instance.allStys.Remove(this);
                Instantiate(GameManagerBeta.Instance.duckPondPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
