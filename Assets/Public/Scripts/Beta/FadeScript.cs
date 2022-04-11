using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    public void showUI()
    {
        myUIGroup.alpha = 1;
    }
    public void HideUI()
    {
        myUIGroup.alpha = 0;
    }
}
