using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBaseHitBox : MonoBehaviour
{
    public bool showText = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!showText && other.gameObject.GetComponent<EnemyControllerBeta>())
        {
            Debug.Log("show text");
            showText = true;
            if (FindObjectOfType<FlashingText>())
                FindObjectOfType<FlashingText>().NewMessage("Pond under attack", 5);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.GetComponent<EnemyControllerBeta>())
            showText = false;
        if (other.gameObject.GetComponent<EnemyControllerBeta>())
            showText = true;

    }
}
