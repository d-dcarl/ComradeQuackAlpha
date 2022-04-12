using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControllerBeta : MonoBehaviour
{
    public List<NodeControllerBeta> neighbors;

    [HideInInspector]
    public bool isGoal;
    private float cost;

    [HideInInspector]
    public NodeControllerBeta bestNeighbor;
    protected bool registered;

    public virtual void Start()
    {
        isGoal = false;
        cost = -1;
        RegisterNode();
    }

    public virtual void Update()
    {
        if (!registered)
        {
            RegisterNode();
        }

        // In update so it can handle when they change
        PondControllerBeta pcb = GetComponent<PondControllerBeta>();
        if(pcb != null)
        {
            isGoal = !pcb.isSty;        // Decide which nodes in the network the enemies pathfind towards
        }

        CalculateBestNeighbor();
    }

    public void CalculateBestNeighbor()
    {
        if(isGoal)
        {
            return;
        }

        foreach(NodeControllerBeta n in neighbors)
        {
            if (n.isGoal) {
                float nDist = Vector3.Distance(transform.position, n.transform.position);
                if (bestNeighbor == null || cost > nDist)
                {
                    bestNeighbor = n;
                    cost = nDist;
                }

            }

            else
            {
                if (n.cost > 0)
                {
                    float newCost = n.cost + Vector3.Distance(transform.position, n.transform.position);
                    if(bestNeighbor == null || cost > newCost)
                    {
                        bestNeighbor = n;
                        cost = newCost;
                    }
                }
            }
        }
    }

    public void RegisterNode()
    {
        if(GameManagerBeta.Instance != null)
        {
            if (GameManagerBeta.Instance.allNodes == null)
            {
                GameManagerBeta.Instance.allNodes = new List<NodeControllerBeta>();
            }
            GameManagerBeta.Instance.allNodes.Add(this);
            registered = true;
        }
    }
}
