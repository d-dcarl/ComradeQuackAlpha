using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBanner : MonoBehaviour
{
    public Image banner;
    public Sprite[] banners; // 0 = chonk, 1 = speed, 2 = corporate, 3 = artillery, 4 = balloon
    public float waitTime;


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Big Pig", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Big Pig") == 0 && GameObject.Find("Big Pig(Clone)"))
        {
            PlayerPrefs.SetInt("Big Pig", 1);
            banner.color = Color.white;
            banner.sprite = banners[0];
            StartCoroutine(HideBanner());
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            banner.color = new Color(0, 0, 0, 0);
        }
    }

    IEnumerator HideBanner()
    {
        yield return new WaitForSeconds(waitTime);
        banner.color = new Color(0, 0, 0, 0);
    }
}
