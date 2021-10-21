using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector3[] path;
    public int speed;

    private Vector3 targetPoint;
    private int targetPointIndex;

    // Start is called before the first frame update
    void Start()
    {
        path = new[]
        {
            new Vector3(50, 5, 0),
            new Vector3(0, 5, 0),
            new Vector3(0, -5, 0)
        };
        
        speed = 10;
        targetPoint = path[0];
        targetPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint) < 1)
        {
            targetPointIndex++;
            if (targetPointIndex >= path.Length)
            {
                HurtBase();
                Destroy(this);
            }
            else
            {
                targetPoint = path[targetPointIndex];
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

