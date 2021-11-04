using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comradDuckController : MonoBehaviour
{
    public PondController pondParent;

    //is the comrad in guard mode.
    public bool isTurret = false;
    private bool traveling = false;
    private Vector3 destination;

    private float quackCooldown;
    private float speed = 12;
    private float step = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //QUACK
        if(quackCooldown <= 0)
        {
            //play the quack
            this.gameObject.GetComponent<AudioSource>().Play();
            //set the cooldown to some random value
            //quackCooldown = 2;
            quackCooldown = Random.Range(3, 50);
        }
    }

    void FixedUpdate()
    {
        //update placement cooldown
        if (quackCooldown < 0)
        {
            quackCooldown = 0;
        }
        else
        {
            quackCooldown -= Time.deltaTime;
        }

        //if traveling go to place its supposed to be, then lock it and stop traveling once there
        if (traveling)
        {
            step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, destination, step);

            //got there, stop traveling
            if(transform.position == destination)
            {
                traveling = false;
            }
        }
    }

        private void OnDestroy()
    {
        //cover if following and if turret
        pondParent.duckIsDestoryed();
        //if following remove from list
        if(!isTurret)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerComradControls>().followingDuckDied(this.gameObject);
        }
    }

    //tell this duck to go stand guard somewhere
    public void standGuard(Vector3 place)
    {
        isTurret = true;
        //traveling = true;
        destination = place;
        //tell the duck to look where it is heading
        Quaternion targetRotation = Quaternion.LookRotation(place - this.transform.position);
        this.GetComponent<TurretController>().initialRotation = targetRotation;
    }

}
