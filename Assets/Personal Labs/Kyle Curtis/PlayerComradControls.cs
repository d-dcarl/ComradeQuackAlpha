using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComradControls : MonoBehaviour
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
    private Queue<GameObject> followingComrads;

    //public get private set
    public int numFollowing { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        previewCooldown = placementPreviewCooldown;
        placeholder = new GameObject();

        followingComrads = new Queue<GameObject>();
        numFollowing = 0;
    }
    private void Update()
    {
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

                    //TODO get a comrad from game manager and tell it to stand here.

                    //set the cooldown for placing a turret
                    cooldown = placementCooldown;
                    previewCooldown = placementPreviewCooldown;
                    isInPreview = false;
                }

            }

        }

    }

    public void AddFollowingDuck(GameObject duck)
    {
        //update the data
        numFollowing++;
        followingComrads.Enqueue(duck);

        //change its following distance
        duck.GetComponent<followPlayer>().changeFollowDistance(numFollowing/2);

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
}
