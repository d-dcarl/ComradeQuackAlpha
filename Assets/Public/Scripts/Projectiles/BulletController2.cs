using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController2 : MonoBehaviour
{
    [SerializeField] public int damage = 3;
    [SerializeField] public float lifetime = 5;

    [HideInInspector]
    public float speed = 10;

    [HideInInspector]
    public Vector3 direction;

    private float lifeTimer;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed == 0f)
            speed = 10f;

        transform.Translate(direction * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
       // Debug.Log(lifeTimer);
        if(lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.tag == "Enemy" && other.GetComponent<PigController>())
            {
                other.GetComponent<PigController>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
