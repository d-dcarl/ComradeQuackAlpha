using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DressingRoomController : MonoBehaviour
{
    public GameObject pedestal;
    public float rotateSpeed;
    public GameObject[] quackHats;
    public GameObject quackHatHolder;

    private int quackHatIndex;
    private int setType = 0;// 0 = quacklings, 1 = quack, 2 = bear

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TurnOffHealthBars();
        RotatePedestal();
    }

    private void TurnOffHealthBars()
    {
        Billboard[] healthBars = FindObjectsOfType<Billboard>();
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].gameObject.SetActive(false);
        }
    }

    private void RotatePedestal()
    {
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;

            pedestal.transform.Rotate(Vector3.up, -rotX);
        }
    }

    public void ChangeHat(int dir)
    {
        quackHatIndex += dir;
        if (quackHatIndex >= quackHats.Length)
            quackHatIndex = 0;
        if (quackHatIndex < 0)
            quackHatIndex = quackHats.Length - 1;

        if (quackHatIndex == 0)
        {
            if (quackHatHolder.transform.childCount > 0)
                Destroy(quackHatHolder.transform.GetChild(0).gameObject);
        }
        if (quackHatIndex > 0)
        {
            Instantiate(quackHats[quackHatIndex], quackHatHolder.transform);
        }
    }

    public void SetHat() // 0 = quacklings, 1 = quack, 2 = bear
    {
        if (setType == 0)
        {
            PlayerPrefs.SetInt("quackling_hat", quackHatIndex);
            Debug.Log(PlayerPrefs.GetInt("quackling_hat"));
            foreach(DucklingControllerBeta quackling in FindObjectsOfType<DucklingControllerBeta>())
            {
                quackling.RemoveHat();
                if (quackHatIndex > 0 && FindObjectOfType<CosmeticHandler>())
                {
                    quackling.ChangeHat(FindObjectOfType<CosmeticHandler>().quacklingFittedHats[PlayerPrefs.GetInt("quackling_hat")]);
                }
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
