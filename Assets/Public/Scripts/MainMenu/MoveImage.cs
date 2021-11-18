using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveImage : MonoBehaviour
{
    public float speedX = 0.05f;
    public float speedY = 0.02f;
    private float curX;
    private float curY;

    void Start() {
        curX = GetComponent<Image>().material.mainTextureOffset.x;
        curY = GetComponent<Image>().material.mainTextureOffset.y;
        
    }

    void FixedUpdate () {
        curX += Time.deltaTime * speedX;
        curY += Time.deltaTime * speedY;
        GetComponent<Image>().material.SetTextureOffset("_MainTex", new Vector2(curX, curY));
    }
}
