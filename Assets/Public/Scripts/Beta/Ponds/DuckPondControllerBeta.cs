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
        if(placedInLevel)
        {
            //changes from object type to reference to pond
            pond = Instantiate(pond, new Vector3(this.transform.position.x, this.transform.position.y + 0.13f, this.transform.position.z), Quaternion.identity);
        }
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

                //PondControllerBeta newPCB = Instantiate(GameManagerBeta.Instance.styPrefab, transform.position, transform.rotation).GetComponent<PondControllerBeta>();


                //foreach (PathControllerBeta path in paths)
                //{
                //    if (path.source == this)
                //    {
                //        path.source = newPCB;
                //    }
                //    else if (path.dest == this) {
                //        path.dest = newPCB;
                //    }
                //}

                foreach (StyControllerBeta scb in GameManagerBeta.Instance.allStys)
                {
                    scb.ResetTarget();
                }
                //tell the new pond where the pond (water and shore) is
                //newPCB.setPond(pond);


                Destroy(gameObject);
            }
        }
    }
}
