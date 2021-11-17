using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public GameObject pigPrefab;

    public int pondHealth;
    public int styHealth;

    public Material water;
    public Material mud;

    public bool cameraTransitioning = false;
    public bool isOverheadView = false;

    public List<PondController> ponds;
    public List<TurretController2> turrets;

    public static GameManager Instance;

    public void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            throw new System.Exception("Error: Multiple instances of singleton GameManager.");
        }
    }
}
