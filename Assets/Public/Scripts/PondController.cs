using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondController : MonoBehaviour
{
    public bool isSty;
    public List<PondController> neighbors;

    private int health;

    public float spawnDelay;
    private float spawnTimer;

    public float offsetAmount;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        SetGM();
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
                Debug.Log("Spawn Pig!");
                Vector3 randomOffset = Random.onUnitSphere;
                randomOffset.y = 0;
                randomOffset = randomOffset.normalized;
                if(gm != null)
                {
                    Instantiate(gm.pigPrefab, transform.position + randomOffset * offsetAmount + Vector3.up, Quaternion.identity);
                }
            }
        }
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
    }

    public void ConvertToPond()
    {
        isSty = false;
        GetComponent<Renderer>().material = gm.water;
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
