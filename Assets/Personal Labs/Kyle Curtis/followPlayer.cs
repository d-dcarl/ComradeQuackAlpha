using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    private Transform target = null;
    private float speed = 0;
    private GameObject Player;
    private float step = 0;
    private float distanceAway = 0;
    private Quaternion targetRotation;
    private float moveSpeedDelay = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Player = 
        Player = GameObject.Find("Duck");
        moveSpeedDelay = 2.5f;
        distanceAway = 2.5f;
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
        //only follow in follow mode, is not a turret
        if (!this.gameObject.GetComponent<comradDuckController>().isTurret)
        {
            //update the speed, and how far we step every frame
            speed = Player.GetComponent<PlayerMovement>().moveSpeed - moveSpeedDelay;
            step = speed * Time.deltaTime;

            //find who the player we are following
            target = Player.transform;
            //Player = GameObject.Find("Duck");

            //if we are far enough away follow the player
            if (Vector3.Distance(this.transform.position, target.position) > distanceAway)
            {
                //move ya booty
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

                //transform.position = Vector3.Lerp(transform.position, Vector3.MoveTowards(transform.position, target.transform.position, step), step);

                //transform.Translate((target.position - transform.position).normalized * speed * Time.deltaTime);
            }

            //rotato happens in TurretController
            targetRotation = Quaternion.LookRotation(target.position - this.transform.position);
            this.GetComponent<TurretController>().initialRotation = targetRotation;
        }
    }
}
