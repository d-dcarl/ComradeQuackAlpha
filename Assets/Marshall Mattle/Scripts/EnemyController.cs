using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
                // hurt the base
                Destroy(this);
            }
            else
            {
                targetPoint = path[targetPointIndex].transform.position;
            }
        }

        var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        var targetForward = targetRotation * Vector3.forward;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.01f);
        transform.position += targetForward * (speed * Time.deltaTime);
    }
}

