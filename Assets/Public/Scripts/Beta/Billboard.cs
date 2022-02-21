using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if(Camera.main)
            transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
