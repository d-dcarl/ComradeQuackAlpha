using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float damageDelt;
    public float knockBack;

    public bool go;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            transform.LookAt(target.transform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    public void Attack(GameObject enemy)
    {
        enemy.GetComponent<EnemyControllerBeta>().TakeDamage(damageDelt);
        enemy.GetComponent<Rigidbody>().AddForce(transform.forward * knockBack);
    }

    public void ReachEnd()
    {
        go = false;
        gameObject.SetActive(false);
    }

}
