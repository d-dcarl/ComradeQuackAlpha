using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPondControllerBeta : PondControllerBeta
{
    public StructureControllerBeta structure;   // For now, just the main turret

    protected bool registered;

    [SerializeField] public GameObject pond;
    [SerializeField] public Material waterTexture;

    public void Start()
    {
        registered = false;
        RegisterPond();
        //Instantiate(pond, this.transform.position, Quaternion.identity);
        //make the pond use the clear water texture


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

                foreach (PathControllerBeta path in paths)
                {
                    if (path.source == this)
                    {
                        path.source = newPCB;
                    }
                    else if (path.dest == this) {
                        path.dest = newPCB;
                    }
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
