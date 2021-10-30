using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    private Transform target = null;
    private float speed = 0;
    private GameObject Player;
    private float step = 0;
    private float rotationSpeed = 5;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        //Player = 
        Player = GameObject.Find("Duck");
        //speed = Player.GetComponent<PlayerMovement>().moveSpeed - 5;
        //target = Player.transform;
        //speed = 6;
    }

    // Update is called once per frame
    void Update()
    {
       

        
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        //update the speed, and how far we step every frame
        speed = Player.GetComponent<PlayerMovement>().moveSpeed - 5;
        step = speed * Time.deltaTime;

        //find who the player we are following
        target = Player.transform;
        //Player = GameObject.Find("Duck");
        //if we are far enough away follow the player
        if (Vector3.Distance(this.transform.position, target.position) > 2.5f)
        {
            //move ya booty
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            //transform.Translate((target.position - transform.position).normalized * speed * Time.deltaTime);
        }

        //if we don't have a target, rotate to look at player
        if (!this.GetComponent<TurretController>().HasTarget)
        {
            targetRotation = Quaternion.LookRotation(target.position - this.transform.position);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
