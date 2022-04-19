using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    public GameObject[] targets;
    private int currentTargetIndex = 0;
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
            transform.LookAt(targets[currentTargetIndex].transform);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    public void Attack(GameObject enemy)
    {
        enemy.GetComponent<EnemyControllerBeta>().TakeDamage(damageDelt);
        enemy.GetComponent<Rigidbody>().AddForce(transform.forward * knockBack);
    }

    public void ReachNode()
    {
        currentTargetIndex++;
    }

    public void ReachEnd()
    {
        go = false;
        gameObject.SetActive(false);
    }

}
