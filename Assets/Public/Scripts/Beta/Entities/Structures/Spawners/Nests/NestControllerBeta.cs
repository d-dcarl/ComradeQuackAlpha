using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestControllerBeta : SpawnerControllerBeta
{
    public GameObject duckPrefab;
    public int spawnCap;
    public float spawnDelay;
    protected float spawnTimer;
    
    public bool turnOffAutoSpawn;
    [HideInInspector]
    public int curSpawns; //this is an implicit list of comrades manning turrets AND comrades in the list

    //timers for upgrading the turret
    protected float upgradeTimer = 0;
    protected float upgradeDelay = 1;

    [SerializeField]  public int upgradeLevel = 0;
    private int upgradeCap = 4;

    public override void Start()
    {
        base.Start();
        spawnTimer = spawnDelay;
        spawned = new List<GameObject>();
        curSpawns = 0;
        changeSpawnTime();
    }

    public override void Update()
    {
        base.Update();
        //spawns ducks
        if (!turnOffAutoSpawn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && curSpawns < spawnCap)
            {
                Spawn(duckPrefab);
                spawnTimer = spawnDelay;
            }
        }
        //decrement the upgrade timer
        upgradeTimer -= Time.deltaTime;
    }

    //tells the spawner that it can spawn a new member
    public void newSpawn()
    {
        curSpawns -= 1;
    }

    public override GameObject Spawn(GameObject prefab)
    {
        DucklingControllerBeta newDuckling = base.Spawn(prefab).GetComponent<DucklingControllerBeta>();

        if (newDuckling == null)
        {
            Debug.LogError("Nest must spawn ducklings.");
            return null;
        }

        newDuckling.InitializeDuckling(this);
        return newDuckling.gameObject;
    }

    //uses a ducking on this nest, to upgrade Returns true if upgraded, false otherwise
    public bool AddDuckling()
    {
        // upgrade if we can still upgrade, and the cooldown has passed
        if (upgradeLevel < upgradeCap && upgradeTimer <= 0)
        {
            UpgradeNest();
            return true;
        }
        //do nothing if nest is at upgrade cap
        return false;
    }


    //Acutally upgrade
    public virtual void UpgradeNest()
    {
        //upgrade tracker
        upgradeLevel += 1;
        //upgrade Stats TODO make the time work better
        changeSpawnTime();
        //reset the cooldown
        upgradeTimer = upgradeDelay;
    }

    private void UnUpgradeNest()
    {
        //upgrade tracker
        upgradeLevel -= 1;
        //upgrade Stats TODO make the time work better
        changeSpawnTime();
    }

    private void changeSpawnTime()
    {
        if(upgradeLevel == 1)
        {
            turnOffAutoSpawn = false;
            spawnDelay = 20;
        }
        else if(upgradeLevel == 2)
        {
            turnOffAutoSpawn = false;
            spawnDelay = 17;
        }
        else if (upgradeLevel == 3)
        {
            turnOffAutoSpawn = false;
            spawnDelay = 14;
        }
        else if (upgradeLevel == 4)
        {
            turnOffAutoSpawn = false;
            spawnDelay = 11;
        }
        //our default is the not spawning
        else
        {
            turnOffAutoSpawn = true;
        }
    }

}
