using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    public int score { get; set; } = 6;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        text.text = "X" + score.ToString();
    }

}