using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBeta : MonoBehaviour
{
    public float gravity = -40f;

    public GameObject duckPondPrefab;
    public GameObject styPrefab;

    public PlayerControllerBeta player;

    [HideInInspector]
    public List<NodeControllerBeta> allNodes;

    [HideInInspector]
    public static GameManagerBeta Instance;

    public void Start()
    {
        EnsureUnique();

        allNodes = new List<NodeControllerBeta>();

        gravity = -1 * Mathf.Abs(gravity);      // Just in case someone puts a positive gravity value by accident
        Physics.gravity = new Vector3(0f, gravity, 0f);
    }

    void EnsureUnique()
    {
        if (GameManagerBeta.Instance != null)
        {
            Debug.Log("Error: more than one game manager");
        }
        else
        {
            GameManagerBeta.Instance = this;
        }

        if (player == null)
        {
            Debug.LogError("Error: Must assign player to game manager");
        }
    }

    public void EndGame()
    {
        player.Die();
    }
}
