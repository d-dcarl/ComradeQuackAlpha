using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<GameObject> path;
    public int speed;

    private Vector3 targetPoint;
    private int targetPointIndex;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        targetPoint = path[0].transform.position;
        targetPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            targetPointIndex++;
            if (targetPointIndex >= path.Count)
            {
                HurtBase();
                Destroy(this);
            }
            else
            {
                targetPoint = path[targetPointIndex].transform.position;
            }
        }
        
        transform.LookAt(targetPoint);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }
    
    private void HurtBase()
    {
        //TODO: logic for damaging the base
    }
}

