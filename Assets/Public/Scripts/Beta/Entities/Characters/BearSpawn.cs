using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawn : MonoBehaviour
{
    public GameObject bear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !bear.activeSelf)//condition to spawn bear
        {
            bear.SetActive(true);
        }
    }
}
