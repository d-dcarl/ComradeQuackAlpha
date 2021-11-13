using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComradeController : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] public PondController pondParent;

    //is the comrad in guard mode.
    [HideInInspector]
    [SerializeField] public bool isTurret = false;
    private bool traveling = false;
    private Vector3 destination;

    private float quackCooldown;
    
    private Rigidbody rb;

    private Transform target = null;
    private GameObject Player;
    [SerializeField] public float speed = 6;
    private float step = 0;
    private const float startDistanceAway = 2.5f;
    private float distanceAway = 2.5f;
    private Quaternion targetRotation;
    private float moveSpeedDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Player = GameObject.Find("Duck");
        moveSpeedDelay = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //QUACK
        if (quackCooldown <= 0)
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
            if (rb.position != destination)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb.position, destination, speed * Time.deltaTime);
                rb.MovePosition(destination);
            }
            else
            {
                traveling = false;
            }
            //got there
        }

        if (!isTurret)
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
            this.GetComponent<TurretController2>().initialRotation = targetRotation;
        }
    }

    public void changeFollowDistance(float amountDiff)
    {
        distanceAway = startDistanceAway + amountDiff;
    }

    private void OnDestroy()
    {
        //if following remove from list
        if (!isTurret)
        {
            if (Player == null)
            {
                Player = GameObject.Find("Duck");
            }
            if(Player != null)
            {
                Player.GetComponent<ManageComradesBehaviour>().followingDuckDied(gameObject);
            }
            
            
        }
        //cover if following and if turret
        pondParent.duckIsDestoryed();
        
    }

    //tell this duck to go stand guard somewhere
    public void standGuard(Vector3 place)
    {
        isTurret = true;
        //traveling = true;
        destination = place;
        //tell the duck to look where it is heading
        Quaternion targetRotation = Quaternion.LookRotation(place - this.transform.position);
        TurretController2 tc = GetComponent<TurretController2>();
        tc.initialRotation = targetRotation;
        tc.InitializeTurret();
    }
}
