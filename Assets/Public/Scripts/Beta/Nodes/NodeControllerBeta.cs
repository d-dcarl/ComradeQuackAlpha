using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControllerBeta : MonoBehaviour
{
    public List<NodeControllerBeta> neighbors;

    [HideInInspector]
    public bool isGoal;

    protected bool registered;

    public virtual void Start()
    {
        isGoal = false;

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
