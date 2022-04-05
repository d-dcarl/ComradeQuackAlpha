using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public GameObject[] ToDisable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                foreach(GameObject go in ToDisable)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                foreach (GameObject go in ToDisable)
                {
                    go.SetActive(false);
                }
            }
        }
    }


}
