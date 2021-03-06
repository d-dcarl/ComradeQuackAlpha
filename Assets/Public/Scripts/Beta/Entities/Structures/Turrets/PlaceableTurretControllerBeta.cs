using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTurretControllerBeta : TurretControllerBeta
{
    protected bool placed;

    [SerializeField]
    protected HitboxControllerBeta placementCollider;
    
    protected int upgradeLevel = 0;
    protected int upgradeCap;
    
    public float damage;
    public float knockback;

    //timers for upgrading the turret
    protected float upgradeTimer;
    protected float upgradeDelay = 1;

    public float constructionDelay = 3;
    public bool isUnderConstruction = false;

    public List<TowerUpgrade> upgrades;

    private int turretColor = 0;

    public VFXController constructionVFX;

    [SerializeField]
    private List<GameObject> turretModels;

    [SerializeField]
    private List<GameObject> baseModels;
    
    [SerializeField]
    private List<GameObject> bulletSpawnPoints;

    [SerializeField]
    private List<GameObject> duckPositions;


    private bool wasKilled = false;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;
        hitBox.enabled = false;
        SetTransparent();

        // turret upgrade stuff
        upgradeLevel = 0;
        upgradeCap = upgrades.Count - 1;

        // This is only to visually show that the turret is inactive on spawning
        currentHealth = 0;
        healthBarSlider.value = 0;

        turretColor = 0;
        
        // setup for multiple turret models
        // basically deactivate every set of turret models so they aren't visible and then reactivate the base model
        foreach (var model in turretModels)
            model.SetActive(false);
        head.SetActive(true);
        
        foreach (var b in baseModels)
            b.SetActive(false);
        turretBase.SetActive(true);

        foreach (var d in duckPositions)
            d.SetActive(false);
    }

    public override void Update()
    {
        if(!placed)
        {
            GoToPlacementPos();
        }
        else if(alive && !isUnderConstruction)
        {
            base.Update();
        }
    }

    public override void TakeDamage(float amount)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/weapons/turret/taking_damage", GetComponent<Transform>().position);
        if (!cantDie)
        {
            currentHealth -= amount;
            if (healthBarSlider)
            {
                healthBarSlider.value = currentHealth;
            }
            if (currentHealth <= 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/weapons/turret/destroyed", GetComponent<Transform>().position);
                wasKilled = true;
                Die();
            }
        }
    }

    public void FixedUpdate()
    {
        //the turret upgrade cooldown
        if (upgradeTimer > 0f)
        {
            upgradeTimer -= Time.deltaTime;
        }
    }

    public virtual void GoToPlacementPos()
    {
        if(GameManagerBeta.Instance != null)
        {
            PlayerControllerBeta player = GameManagerBeta.Instance.player;
            Vector3 placementPos = player.transform.position + player.transform.forward * player.placementDistance;

            Vector3 hitNormal = Vector3.up;

            if (Physics.Raycast(new Vector3(placementPos.x, 50f, placementPos.z), Vector3.down, out var hit, 100f, LayerMask.GetMask("Ground")))
            {
                placementPos.y = hit.point.y;
                hitNormal = hit.normal;
            }
            else
            {
                placementPos.y = 0;
            }

            transform.position = placementPos;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal) * player.transform.rotation;

            hitBox.transform.rotation = transform.rotation;
        }
    }

    public bool IsValidPlacementLocation
    {
        get 
        {
            foreach (var o in placementCollider.tracked)
            {
                if (ComparePlacementTags(o))
                {
                    return false;
                }
            }
        
            return true;
        }
    }

    private bool ComparePlacementTags(GameObject obj)
    {
        return obj.CompareTag("Turret") || obj.CompareTag("Flying Turret") || obj.CompareTag("Player Structure") || obj.CompareTag("Enemy Structure") || obj.CompareTag("Environment") || obj.CompareTag("Nest");
    }

    public void PlaceTurret()
    {
        placed = true;
        hitBox.enabled = true;

        var placementHitbox = placementCollider.GetComponent<BoxCollider>();
        placementHitbox.enabled = false;

        currentHealth = 0;
        healthBarSlider.value = 0;
        //ActivateTurret();       // For now
    }

    public void ActivateTurret()
    {
        //update our values
        alive = true;
        //update health
        currentHealth = maxHealth;
        healthBarSlider.value = maxHealth;
        //make it so that it doesnt upgrade as we activate
        upgradeTimer = upgradeDelay;
        //make the color the activated color
        SetOpaque();

        duck.SetActive(true);
        SetUpgrade(upgrades[upgradeLevel]);
    }

    private void DeactivateTurret()
    {
        alive = false;
        SetTransparent();
        duck.SetActive(false);
    }

    public void SetTransparent()
    {
        int i = 0;
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (!r.gameObject.CompareTag("VFX"))
                r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
            Color oldColor = r.material.color;
            r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
            i++;
        }
        gameObject.layer = LayerMask.NameToLayer("Dead Player Structure");
    }

    public void SetOpaque()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (!r.gameObject.CompareTag("VFX"))
                r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }
        gameObject.layer = LayerMask.NameToLayer("Player Structure");
    }

    public override void Die()
    {
        if(!wasKilled)
        {
            DeactivateTurret();
        }
        else
        {
            base.Die();
        }
    }

    public void lookedAt(bool isLookedAt)
    {
        if (isLookedAt)
        {
            //set activate turret color
            if (!this.alive)
            {
                if (turretColor != 1)
                {
                    turretColor = 1;
                    SetHoverColor(1);
                }
            }
            //otherwise upgrade turret color
            else if (upgradeLevel < upgradeCap && upgradeTimer <= 0)
            {
                if (turretColor != 2)
                {
                    turretColor = 2;
                    SetHoverColor(2);
                }
            }
            //neither turret color
            else
            {
                if (turretColor != 3)
                {
                    turretColor = 3;
                    SetHoverColor(3);
                }
            }
        }
        else
        {
            if (turretColor != 0)
            {
                turretColor = 0;
                SetHoverColor(0);
            }
        }


    }
    private void SetHoverColor(int colorToSet)
    {
        //steal from set transparent
        //float redChange = 0;
        //float greenChange = 0;
        //float blueChange = 0;
        
        //set activate turret color
       switch(colorToSet)
        {
            case 1:
                //greenChange = 255;
                break;
            case 2:
                //blueChange = 255;
                break;
            case 3:
                //redChange = 255;
                break;
            default:
                if (alive)
                {
                    SetOpaque();
                }
                else
                {
                    SetTransparent();
                }
                return;
        }
        //actually change the material
        //clear color
        if (!alive)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                if (!r.gameObject.CompareTag("VFX"))
                    r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
                Color oldColor = r.material.color;
                r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a);
            }
        }
        //opaque color
        else
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                if (!r.gameObject.CompareTag("VFX"))
                    r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
                Color oldColor = r.material.color;
                r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a);
            }
        }

    }


    //uses a ducking on this turret, either to upgrade or activate. Returns true if upgraded or activated, false otherwise
    public bool AddDuckling()
    {
        //activate turret if inactive
        if (!this.alive && !isUnderConstruction)
        {
            StartConstruction(ConstructionType.Activate);
            return true;
        }
        //otherwise upgrade if we can still upgrade, and the cooldown has passed
        else if (upgradeLevel < upgradeCap && !isUnderConstruction)
        {
            StartConstruction(ConstructionType.Upgrade);
            return true;
        }
        //lookedAt(false);
        //do nothing if turret is at upgrade cap
        return false;
    }


    //removes a duckling from this turret
    public int RemoveDuckling()
    {
        int removed = 0;
        //can't de-upgrade
        if(isUnderConstruction)
        {
            return -1;
        }
        if(upgradeLevel == 0)
        {
            if(alive)
            {
                //kill the turret
                StartConstruction(ConstructionType.Deactivate);
            }
            else
            {
                //destroy the turret
                Destroy(this.gameObject, constructionDelay + .5f);
                removed = 1; 
            }
        }
        StartConstruction(ConstructionType.Downgrade);

        return removed;
    }

    protected virtual void DowngradeTurret()
    {
        //if can un-upgrade do it
        upgradeLevel = upgradeLevel > 0 ? upgradeLevel - 1 : 0;

        this.transform.localScale = new Vector3(this.transform.localScale.x - 0.05f, this.transform.localScale.y - 0.05f, this.transform.localScale.z - 0.05f);

        SetModels();
        
        duck.SetActive(alive);

        SetUpgrade(upgrades[upgradeLevel]);
    }

    private void StartConstruction(ConstructionType type)
    {
        isUnderConstruction = true;
        constructionVFX.StartVFX();
        StartCoroutine(EndConstruction(type));
    }

    private IEnumerator EndConstruction(ConstructionType type)
    {
        yield return new WaitForSeconds(constructionDelay);

        isUnderConstruction = false;
        constructionVFX.StopVFX();
        switch (type)
        {
            case ConstructionType.Activate:
                ActivateTurret();
                break;
            case ConstructionType.Deactivate:
                DeactivateTurret();
                break;
            case ConstructionType.Downgrade:
                DowngradeTurret();
                break;
            case ConstructionType.Upgrade:
                UpgradeTurret();
                break;
            default:
                Debug.Log("Must specify what kind of construction is taking place on the turret. This shouldn't ever happen.");
                break;
        }
    }

    protected virtual void UpgradeTurret()
    {
        // keeping this until we have actual visual indicators of upgrades
        this.transform.localScale = new Vector3(this.transform.localScale.x + 0.05f, this.transform.localScale.y + 0.05f, this.transform.localScale.z + 0.05f);

        //reset the cooldown
        upgradeTimer = upgradeDelay;
        //heal
        currentHealth = maxHealth;
        healthBarSlider.value = maxHealth;
        upgradeLevel++;
        
        SetModels();

        SetUpgrade(upgrades[upgradeLevel]);
    }

    protected virtual void SetUpgrade(TowerUpgrade upgrade)
    {
        damage = upgrade.damage;
        knockback = upgrade.knockback;
        targetRange.range = upgrade.range;
        fireRate = upgrade.fireRate;

        hitBox.center = upgrade.hitboxCenter;
        hitBox.size = upgrade.hitboxSize;
    }

    protected virtual void SetModels()
    {
        head.SetActive(false);
        if (upgradeLevel < turretModels.Count)
        {
            var newHead = turretModels[upgradeLevel];
            newHead.transform.rotation = head.transform.rotation;
            head = newHead;
        }

        head.SetActive(true);

        turretBase.SetActive(false);
        if (upgradeLevel < baseModels.Count)
            turretBase = baseModels[upgradeLevel];
        turretBase.SetActive(true);

        gun.SetActive(false);
        if (upgradeLevel < bulletSpawnPoints.Count)
            gun = bulletSpawnPoints[upgradeLevel];
        gun.SetActive(true);

        duck.SetActive(false);
        if (upgradeLevel < duckPositions.Count)
        {
            var newDuck = duckPositions[upgradeLevel];
            newDuck.transform.rotation = duck.transform.rotation;
            duck = newDuck;
        }

        duck.SetActive(true);
    }

    private enum ConstructionType
    {
        Activate,
        Deactivate,
        Upgrade,
        Downgrade
    }
}
