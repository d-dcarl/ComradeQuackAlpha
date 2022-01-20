using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestControllerBeta : SpawnerControllerBeta
{
    //timers for upgrading the turret
    protected float upgradeTimer = 0;
    protected float upgradeDelay = 1;

    private int upgradeLevel = 0;
    private int upgradeCap = 3;


    public override GameObject Spawn()
    {
        DucklingControllerBeta newDuckling = base.Spawn().GetComponent<DucklingControllerBeta>();

        if(newDuckling == null)
        {
            Debug.LogError("Nest must spawn ducklings.");
            return null;
        }

        newDuckling.InitializeDuckling(this);
        return newDuckling.gameObject;
    }

    public override void Update()
    {
        base.Update();
        if (!turnOffAutoSpawn)
        {
            CleanSpawnedList();
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && curSpawns < spawnCap)
            {
                Spawn();
                spawnTimer = spawnDelay;
            }
        }
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
    private void UpgradeNest()
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
            //spawnDelay = 2;
        }
        else if(upgradeLevel == 2)
        {

        }
        else if (upgradeLevel == 3)
        {

        }
        //our default is the min spawn time
        else
        {

        }
    }
}
