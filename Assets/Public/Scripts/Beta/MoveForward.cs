using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    float lifeCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
