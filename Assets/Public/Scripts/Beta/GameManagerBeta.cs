using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    int numOfPonds = 0;

    public void Start()
    {
        EnsureUnique();

        allNodes = new List<NodeControllerBeta>();

        gravity = -1 * Mathf.Abs(gravity);      // Just in case someone puts a positive gravity value by accident
        Physics.gravity = new Vector3(0f, gravity, 0f);

        numOfPonds = FindObjectsOfType<PondControllerBeta>().Length;
    }

    private void Update()
    {
        if (numOfPonds > 0)
        {
            GetNumOfPonds();
        }

        foreach (NodeControllerBeta n in allNodes)
        {
            n.ResetCost();
        }

        for (int i = 0; i < allNodes.Count; i++)
        {
            foreach (NodeControllerBeta n in allNodes)
            {
                n.CalculateBestNeighbor();
            }
        }
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

    private void GetNumOfPonds()
    {
        numOfPonds = 0;
        foreach(PondControllerBeta pond in FindObjectsOfType<PondControllerBeta>())
        {
            if (!pond.isSty)
                numOfPonds++;
        }
        if (numOfPonds < 1)
        {
           StartCoroutine(WaitToGameOver());
        }
    }

    IEnumerator WaitToGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOver");
    }
}
