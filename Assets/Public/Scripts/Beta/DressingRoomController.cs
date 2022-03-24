using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingRoomController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnOffHealthBars();
    }

    private void TurnOffHealthBars()
    {
        Billboard[] healthBars = FindObjectsOfType<Billboard>();
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].gameObject.SetActive(false);
        }
    }
}
