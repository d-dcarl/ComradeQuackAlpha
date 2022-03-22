using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableNestControllerBeta : NestControllerBeta
{
    private bool placed;
    protected CapsuleCollider hitBox;
    private bool isUnderConstruction = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        hitBox = GetComponent<CapsuleCollider>();
        placed = true;
        alive = true;
        hitBox.enabled = false;
        //SetTransparent();

        // This is only to visually show that the turret is inactive on spawning
        currentHealth = 0;
        healthBarSlider.value = 0;
    }

    private void SetTransparent()
    {
        int i = 0;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
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
            r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }
        gameObject.layer = LayerMask.NameToLayer("Player Structure");
    }

    // Update is called once per frame
    public override void  Update()
    {
        if (!placed)
        {
            GoToPlacementPos();
        }
        else if (alive && !isUnderConstruction)
        {
            base.Update();
        }
    }

    private void GoToPlacementPos()
    {
        if (GameManagerBeta.Instance != null)
        {
            PlayerControllerBeta player = GameManagerBeta.Instance.player;
            Vector3 playerPos = player.transform.position;
            //.75
            transform.position = new Vector3(playerPos.x, 0.75f, playerPos.z) + player.transform.forward * player.placementDistance;
        }
    }

    public void PlaceNest()
    {
        placed = true;
        hitBox.enabled = true;
        currentHealth = 0;
        healthBarSlider.value = 0;
        //ActivateNest();       // For now
    }

    public void ActivateNest()
    {
        //update our values
        alive = true;
        //update health
        currentHealth = maxHealth;
        healthBarSlider.value = maxHealth;
        //make it so that it doesnt upgrade as we activate
        upgradeTimer = upgradeDelay;
        //make the color the activated color
        //SetOpaque();
    }

    public override void UpgradeNest()
    {
        base.UpgradeNest();
        ActivateNest();
    }

}
