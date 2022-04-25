using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashingText : MonoBehaviour
{
    public float flashInterval;

    TextMeshProUGUI textUI;
    string currentMessage = "";
    public bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            textUI.text = "";
            currentMessage = "";
            isFlashing = false;
        }
    }

    public void NewMessage(string message, int flashCount)
    {
        currentMessage = message;
        textUI.text = message;
        if (!isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashText(flashCount));
        }
    }

    IEnumerator FlashText(int count)
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(flashInterval);
            textUI.text = "";
            yield return new WaitForSeconds(flashInterval * 2);
            textUI.text = currentMessage;
            if (!isFlashing)
                break;
        }
        textUI.text = "";
        currentMessage = "";
        isFlashing = false;
    }
}
