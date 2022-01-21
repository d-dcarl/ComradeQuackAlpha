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
        ResetTarget();
        registered = false;
        RegisterSty();

        //makes the pond
        if (placedInLevel)
        {
            //changes from object type to reference to pond
            pond = Instantiate(pond, new Vector3(this.transform.position.x, this.transform.position.y + 0.13f, this.transform.position.z), Quaternion.identity);
            //make the pond use the dirty water texture
            changePondTexture();
        }
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

    public void ResetTarget()
    {
        bestNeighbor = null;
        totalDistanceToPond = -1f;
    }

    public PondControllerBeta GetTargetPond()
    {
        return bestNeighbor;
    }

    List <PondControllerBeta> GetNeighbors()
    {
        List<PondControllerBeta> neighbors = new List<PondControllerBeta>();
        foreach(PathControllerBeta path in paths)
        {
            if(path.source == this)
            {
                neighbors.Add(path.dest);
            }
            else if(path.dest == this)
            {
                neighbors.Add(path.source);
            }
        }
        return neighbors;
    }

    void UpdateBestNeighbor()
    {
        foreach(PondControllerBeta pcb in GetNeighbors())
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
                // Unregister
                GameManagerBeta.Instance.allStys.Remove(this);

                PondControllerBeta newPCB = Instantiate(GameManagerBeta.Instance.duckPondPrefab, transform.position, transform.rotation).GetComponent<PondControllerBeta>();

                foreach (PathControllerBeta path in paths)
                {
                    if(path.source == this)
                    {
                        path.source = newPCB;
                    }
                    else if(path.dest == this)
                    {
                        path.dest = newPCB;
                    }
                }

                foreach (StyControllerBeta scb in GameManagerBeta.Instance.allStys)
                {
                    scb.ResetTarget();
                }

                //tell the new pond where the pond (water and shore) is
                newPCB.setPond(pond);

                Destroy(gameObject);
            }
        }
    }
}
