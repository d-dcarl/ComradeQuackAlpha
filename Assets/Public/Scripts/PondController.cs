using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondController : MonoBehaviour
{
    public bool isSty;
    public List<PondController> neighbors;

    private float nearestPondDist;
    [HideInInspector]
    public PondController pointTo;

    private int health;

    public float spawnDelay;
    private float spawnTimer;

    public float offsetAmount;

    private GameManager gm;

    //these lines were added to spawn comrads
    //this keeps track of the number of comrads this pond has spawned
    private int numComrads;
    //the max number of comrads this pond can spawn (1 for testing, should be something like 3)
    [SerializeField] public int maxComrads;
    //if the player is touching the pond and can spawn from it
    private bool playerTouching;
    //the type of comrad this pond spawns
    [SerializeField] public GameObject comrad;

    GameObject curDuck;

    private float cooldown = 0;
    [SerializeField] public float recruitCooldown = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        SetGM();
        numComrads = 0;
        playerTouching = false;


    }

    // Update is called once per frame
    void Update()
    {
        if(gm == null)
        {
            SetGM();
        }
        if(isSty)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                spawnTimer = spawnDelay;
                Vector3 randomOffset = Random.onUnitSphere;
                randomOffset.y = 0;
                randomOffset = randomOffset.normalized;
                if(gm != null)
                {
                    PigController pig = Instantiate(gm.pigPrefab, transform.position + randomOffset * offsetAmount + Vector3.up, Quaternion.identity).GetComponent<PigController>();
                    pig.homeSty = this;
                    pig.target = pointTo;
                }
            }
        }



        //check if the pond should spawn a comrad
        //it has to not be a sty, and the player has to be touching, and the key G has to be down, and we have to have less than the max number of comrads
        if (!isSty && playerTouching && Input.GetKey(KeyCode.G) && numComrads < maxComrads)
        {
            
            //can the player have any more following ducks
            if(GameObject.FindWithTag("Player").GetComponent<PlayerComradControls>().canAddDuck)
            {
                //spawn after cooldown
                if (cooldown <= 0)
                {
                    //spawn the duck
                    //curDuck = Instantiate<GameObject>(comrad, GameObject.Find("Player").transform);
                    Vector3 randomOffset = Random.onUnitSphere;
                    randomOffset.y = 0;
                    randomOffset = randomOffset.normalized;
                    curDuck = Instantiate<GameObject>(comrad, transform.position + randomOffset * offsetAmount + Vector3.up, Quaternion.identity);
                    curDuck.GetComponent<comradDuckController>().pondParent = this;

                    //update data and cooldown
                    numComrads++;
                    cooldown = recruitCooldown;
                    GameObject.FindWithTag("Player").GetComponent<PlayerComradControls>().AddFollowingDuck(curDuck);
                    //
                }
            }
        
        }
    }

    private void FixedUpdate()
    {
        //update placement cooldown, this prevents overflow
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if the player entered we set 
        if (other.gameObject.tag == "Player")
        {
            playerTouching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerTouching = false;
        }
    }

    public static void setPolicy()
    {
        foreach (PondController pond in GameManager.Instance.ponds)
        {
            pond.pointTo = null;
            pond.nearestPondDist = -1;
        }

        // Iterate V times, where V is the number of ponds
        for(int v = 0; v < GameManager.Instance.ponds.Count; v++)
        {
            foreach (PondController pond in GameManager.Instance.ponds)
            {
                if (!pond.isSty)
                {
                    pond.nearestPondDist = 0f;
                    pond.pointTo = pond;
                }
                else
                {
                    PondController bestNeighbor = null;
                    float bestDist = -1;

                    for (int i = 0; i < pond.neighbors.Count; i++)
                    {
                        // Look for neighbors that already know their nearest distance
                        PondController neighbor = pond.neighbors[i];
                        if (neighbor.nearestPondDist >= 0f)
                        {
                            float dist = Vector3.Distance(neighbor.transform.position, pond.transform.position);
                            float totalDist = dist + neighbor.nearestPondDist;

                            if (bestDist < 0f || dist < bestDist)
                            {
                                bestNeighbor = neighbor;
                                bestDist = dist;
                            }
                        }

                    }
                    if (bestNeighbor != null)
                    {
                        pond.pointTo = bestNeighbor;
                        pond.nearestPondDist = bestDist;
                    }
                }
            }
        }
    }

    //decrement the number of ducks
    public void duckIsDestoryed()
    {
        numComrads--;
    }

    void SetGM()
    {
        gm = GameManager.Instance;
        if (gm != null)
        {
            if (gm.ponds == null)
            {
                gm.ponds = new List<PondController>();
            }
            gm.ponds.Add(this);
            PondController.setPolicy();

            health = isSty ? gm.styHealth : gm.pondHealth;
            GetComponent<Renderer>().material = isSty ? gm.mud : gm.water;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            SwitchTeams();
        }
    }

    public void ConvertToSty()
    {
        isSty = true;
        GetComponent<Renderer>().material = gm.mud;
        PondController.setPolicy();
    }

    public void ConvertToPond()
    {
        isSty = false;
        GetComponent<Renderer>().material = gm.water;
        PondController.setPolicy();
    }

    public void SwitchTeams()
    {
        if(isSty)
        {
            ConvertToPond();
        } else
        {
            ConvertToSty();
        }
    }
}
