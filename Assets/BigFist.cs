using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFist : MonoBehaviour
{
    public GameObject pig;
    public void OnCollison(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
