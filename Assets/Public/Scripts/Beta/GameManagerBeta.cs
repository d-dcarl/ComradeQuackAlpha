using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBeta : MonoBehaviour
{
    public float gravity = -40f;

    public GameObject duckPondPrefab;
    public GameObject styPrefab;

    [HideInInspector]
    public static GameManagerBeta Instance;

    public void Start()
    {
        if(GameManagerBeta.Instance != null)
        {
            Debug.Log("Error: more than one game manager");
        }
        else
        {
            GameManagerBeta.Instance = this;
        }

        gravity = -1 * Mathf.Abs(gravity);      // Just in case someone puts a positive gravity value by accident
        Physics.gravity = new Vector3(0f, gravity, 0f);
    }
}
