using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTurretControllerBeta : TurretControllerBeta
{
    protected bool placed;
    private int upgradeLevel = 0;
    private int upgradeCap;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;
        SetTransparent();

        //turret upgrade stuff
        upgradeLevel = 0;
        upgradeCap = 10;

        //This is only to visually show that the turret in inactive on spawning
        currentHealth = 0;
        healthBarSlider.value = 0;
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
        //ActivateTurret();       // For now
    }

    public void ActivateTurret()
    {
        alive = true;
        currentHealth = maxHealth;
        healthBarSlider.value = maxHealth;
        SetOpaque();
    }

    public void SetTransparent()
    {
        hitBox.enabled = false;

        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
            Color oldColor = r.material.color;
            r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
        }
    }

    public void SetOpaque()
    {
        hitBox.enabled = true;

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


    public virtual void SetHoverColor()
    {
        //TODO: Set turret outline to [INSERT COLOR HERE] to activate turret
        if (!this.alive)
        {
            //foreach (Renderer r in GetComponentsInChildren<Renderer>())
            //{
            //    //https://www.gamedev.net/blogs/entry/2264832-highlight-in-unity/ DO this 

            //}
        }
        //TODO: set turret outline color for upgrading
        //TODO: set turret outline color for unable to upgrade or add duckling

    }

    public virtual bool AddDuckling()
    {
        //activate turret if inactive
        if (!this.alive)
        {
            //TODO Make sure the duckling doesn't actually die until turret is dead too 
            ActivateTurret();
            return true;
        }
        else if(upgradeLevel < upgradeCap)
        {
            UpgradeTurret();
            return true;
        }
        return false;
        //upgrade turret if active
        //do nothing if turret is at upgrade cap
    }

    //TODO Acutally upgrade turret and give turret a cooldown for upgrading
    protected virtual void UpgradeTurret()
    {
        upgradeLevel += 1;
        this.fireRate -= 0.1f;
        this.transform.localScale = new Vector3(this.transform.localScale.x + 0.05f, this.transform.localScale.y + 0.05f, this.transform.localScale.z + 0.05f);
    }
}
