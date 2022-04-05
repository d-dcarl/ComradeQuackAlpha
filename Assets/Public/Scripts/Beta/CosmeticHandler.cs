using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticHandler : MonoBehaviour
{
    public GameObject[] quacklingFittedHats;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < Object.FindObjectsOfType<CosmeticHandler>().Length; i++)
        {
            if (Object.FindObjectsOfType<CosmeticHandler>()[i] != this)
            {
                if (Object.FindObjectsOfType<CosmeticHandler>()[i].name == gameObject.name)
                {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
