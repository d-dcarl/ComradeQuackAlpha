using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPondControllerBeta : PondControllerBeta
{
    public StructureControllerBeta structure;   // For now, just the main turret

    protected bool registered;

    public void Start()
    {
        registered = false;
        RegisterPond();
    }

    public void Update()
    {
        if(!registered)
        {
            RegisterPond();
        }

        CheckDead();
    }

    void RegisterPond()
    {
        if (GameManagerBeta.Instance != null)
        {
            if (GameManagerBeta.Instance.allPonds == null)
            {
                GameManagerBeta.Instance.allPonds = new List<DuckPondControllerBeta>();
            }
            GameManagerBeta.Instance.allPonds.Add(this);

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
                GameManagerBeta.Instance.allPonds.Remove(this);

                // If this was the last pond, game over
                if(GameManagerBeta.Instance.allPonds.Count < 1)
                {
                    GameManagerBeta.Instance.EndGame();
                }

                PondControllerBeta newPCB = Instantiate(GameManagerBeta.Instance.styPrefab, transform.position, transform.rotation).GetComponent<PondControllerBeta>();
                newPCB.neighbors = new List<PondControllerBeta>();

                foreach (PondControllerBeta neighbor in neighbors)
                {
                    neighbor.neighbors.Remove(this);

                    neighbor.neighbors.Add(newPCB);
                    newPCB.neighbors.Add(neighbor);
                }

                foreach (StyControllerBeta scb in GameManagerBeta.Instance.allStys)
                {
                    scb.ResetTarget();
                }
                Destroy(gameObject);
            }
        }
    }
}
