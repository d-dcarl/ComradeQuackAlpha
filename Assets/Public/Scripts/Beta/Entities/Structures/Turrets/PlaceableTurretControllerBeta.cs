using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTurretControllerBeta : TurretControllerBeta
{
    protected bool placed;
    private int upgradeLevel = 0;
    private int upgradeCap;

    //timers for upgrading the turret
    protected float upgradeTimer;
    protected float upgradeDelay = 1;

    private int turretColor = 0;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;
        hitBox.enabled = false;
        SetTransparent();

        //turret upgrade stuff
        upgradeLevel = 0;
        upgradeCap = 10;

        //This is only to visually show that the turret in inactive on spawning
        currentHealth = 0;
        healthBarSlider.value = 0;

        turretColor = 0;
    }



    public override void Update()
    {
        if(!placed)
        {
            GoToPlacementPos();
        }
        else if(alive)
        {
            base.Update();
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

    void GoToPlacementPos()
    {
        if(GameManagerBeta.Instance != null)
        {
            PlayerControllerBeta player = GameManagerBeta.Instance.player;
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, 0f, playerPos.z) + player.transform.forward * player.placementDistance;
        }
    }

    public bool ValidPlacementLocation()
    {
        return true;
    }

    public void PlaceTurret()
    {
        placed = true;
        hitBox.enabled = true;
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
        hitBox.enabled = true;
    }

    public void SetTransparent()
    {
        int i = 0;
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
            Color oldColor = r.material.color;
            r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
            i++;
        }
    }

    public void SetOpaque()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Standard");
        }
    }

    public override void Die()
    {
        alive = false;
        SetTransparent();
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
        float redChange = 0;
        float greenChange = 0;
        float blueChange = 0;
        
        //set activate turret color
       switch(colorToSet)
        {
            case 1:
                greenChange = 255;
                break;
            case 2:
                blueChange = 255;
                break;
            case 3:
                redChange = 255;
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
                r.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                Color oldColor = r.material.color;
                r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a);
            }
        }
        //opaque color
        else
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material.shader = Shader.Find("Standard");
                Color oldColor = r.material.color;
                r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a);
            }
        }

    }


    //uses a ducking on this turret, either to upgrade or activate. Returns true if upgraded or activated, false otherwise
    public bool AddDuckling()
    {
        //activate turret if inactive
        if (!this.alive)
        {
            ActivateTurret();
            return true;
        }
        //otherwise upgrade if we can still upgrade, and the cooldown has passed
        else if (upgradeLevel < upgradeCap && upgradeTimer <= 0)
        {
            UpgradeTurret();
            return true;
        }
        //lookedAt(false);
        //do nothing if turret is at upgrade cap
        return false;
    }

    //TODO Acutally upgrade turret
    private void UpgradeTurret()
    {
        //upgrade tracker
        upgradeLevel += 1;
        //upgrade Stats TODO make it
        this.fireRate -= 0.1f;
        this.transform.localScale = new Vector3(this.transform.localScale.x + 0.05f, this.transform.localScale.y + 0.05f, this.transform.localScale.z + 0.05f);
        //reset the cooldown
        upgradeTimer = upgradeDelay;
        //heal
        currentHealth = maxHealth;
        healthBarSlider.value = maxHealth;
    }
}
