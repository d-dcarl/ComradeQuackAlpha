using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public int coinValue = 1;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Bear")
        {
            ScoreManager.instance.ChangeScore(coinValue);
            Destroy(gameObject);
        }
    }
}
