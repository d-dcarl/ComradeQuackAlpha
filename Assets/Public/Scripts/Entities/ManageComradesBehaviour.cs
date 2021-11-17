using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageComradesBehaviour : MonoBehaviour
{
    //Public
    [SerializeField] public GameObject placeableTurret;
    [SerializeField] public GameObject placementTurret;
    [SerializeField] public float placementCooldown = 1.0f;
    [SerializeField] public float placementPreviewCooldown = 0.7f;
    [SerializeField] public float placementDistance = 2.0f;


    [SerializeField] public int maxComrads = 10;

    public float turnSmoothTime = 0.1f;

    [SerializeField] public bool canAddDuck = true;


    //Private
    private float cooldown = 0;
    private float previewCooldown;
    private bool isInPreview = false;
    private GameObject placeholder;
    private ComradeController curComradeController;

    //the comrads currently following the player
    private List<GameObject> followingComrads;

    //a list of comrads to recruit
    private List<ComradeController> recruitableComrads;

    //public get private set
    public int numFollowing { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        previewCooldown = placementPreviewCooldown;
        placeholder = new GameObject();

        followingComrads = new List<GameObject>();
        numFollowing = 0;

        recruitableComrads = new List<ComradeController>();
    }
    private void Update()
    {
        //placing ducks in turrets
        //Input for placing a turret
        if (Input.GetKey(KeyCode.F))
        {
            if (cooldown <= 0)
            {

                if (!isInPreview)
                {
                    //Instantiate the placement turret
                    placeholder = Instantiate<GameObject>(placementTurret, this.transform);
                }
                isInPreview = true;

                //wait a minimum time before accepting inputs
                if (previewCooldown <= 0)
                {
                    //delete the placement turret
                    Destroy(placeholder);

                    /*
                    //instantiate the actual turret
                    Vector3 newPosition = new Vector3(transform.position.x + (placementDistance * this.transform.forward.x), transform.position.y, transform.position.z + (placementDistance * this.transform.forward.z));
                    Instantiate<GameObject>(placeableTurret, newPosition, this.transform.rotation);
                    */

                    //TODO tell comrad to pilot turret
                    if (numFollowing > 0)
                    {
                        //remove the duck thats been following the longest
                        GameObject curDuck = followingComrads[0];
                        followingComrads.Remove(curDuck);

                        Vector3 newPosition = new Vector3(transform.position.x + (placementDistance * this.transform.forward.x), transform.position.y, transform.position.z + (placementDistance * this.transform.forward.z));
                        curDuck.GetComponent<ComradeController>().standGuard(newPosition);
                    }

                    //set the cooldown for placing a turret
                    cooldown = placementCooldown;
                    previewCooldown = placementPreviewCooldown;
                    isInPreview = false;
                }

            }

        }

        //recruit comrades in zone
        if (Input.GetKey(KeyCode.G))
        {
            //as long as there are recruitable comrades
            while(recruitableComrads.Count != 0)
            {
                //or we can't have more following
                if(numFollowing >= maxComrads)
                {
                    break;
                }
                //otherwise remove a recruitable comrade from the list and tell it to follow
                curComradeController = recruitableComrads[0];
                recruitableComrads.Remove(curComradeController);
                //follows when not a turret
                curComradeController.isTurret = false;
                
            }
        }
    }

    //if a following duck died remove it from the list
    public void followingDuckDied(GameObject duck)
    {
        //remove the duck from the list, if we did do stuff
        if (followingComrads.Remove(duck))
        {
            //update the data trackers
            numFollowing--;
            if (numFollowing < maxComrads)
            {
                canAddDuck = true;
            }
        }
    }

    public void AddFollowingDuck(GameObject duck)
    {
        //update the data
        numFollowing++;
        //add to the end of the list
        followingComrads.Add(duck);

        //change its following distance
        duck.GetComponent<ComradeController>().changeFollowDistance(numFollowing/2);

        if(numFollowing >= maxComrads)
        {
            canAddDuck = false;
        }
    }

    void FixedUpdate()
    {
        //update placement cooldown
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
        //update preview cooldown
        if (previewCooldown < 0)
        {
            previewCooldown = 0;
        }
        else if (isInPreview)
        {
            previewCooldown -= Time.deltaTime;
        }

    }

    //when a comrad enters the trigger adds then to the recruitable list if they aren't following
    private void OnTriggerEnter(Collider other)
    {
        //get the comrade's controller, if they are a comrad
        curComradeController = other.gameObject.GetComponent<ComradeController>();
        if(curComradeController != null)
        {
            //now check if its a turret, then it can be recruited
            if(curComradeController.isTurret)
            {
                recruitableComrads.Add(curComradeController);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //get the comrade's controller, if they are a comrad
        curComradeController = other.gameObject.GetComponent<ComradeController>();
        if (curComradeController != null)
        {
            //now check if its a turret, then it can no longer be recruited
            if (curComradeController.isTurret)
            {
                recruitableComrads.Remove(curComradeController);
            }
        }
    }
}
