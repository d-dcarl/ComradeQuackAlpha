using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawn : MonoBehaviour
{
    public GameObject bear;
    public float secondsAfterSpawn;
    public float spawnHeight;
    public bool canSpawn;
    public GameObject bearUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && Input.GetKeyDown(KeyCode.B) && !bear.activeSelf)//condition to spawn bear
        {
            bear.transform.position = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
            bear.SetActive(true);
            StartCoroutine(bearGo());
            canSpawn = false;
            bearUI.SetActive(false);
        }
    }

    IEnumerator bearGo()
    {
        yield return new WaitForSeconds(secondsAfterSpawn);
        bear.GetComponent<BearController>().go = true;
    }
}
