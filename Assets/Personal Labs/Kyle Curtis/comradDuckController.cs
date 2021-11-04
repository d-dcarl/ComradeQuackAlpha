using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comradDuckController : MonoBehaviour
{
    public PondController pondParent;

    //is the comrad in guard mode.
    public bool isTurret = false;
    private bool traveling = false;
    private Transform destination;

    private float quackCooldown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO go to place its supposed to be, then lock it
        if(traveling)
        {
            //got there
        }

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
    }

        private void OnDestroy()
    {
        //cover if following and if turret
        pondParent.duckIsDestoryed();
    }

    //tell this duck to go stand guard somewhere
    public void standGuard(Transform place)
    {
        isTurret = true;
        traveling = true;
        destination = place;
    }

}
