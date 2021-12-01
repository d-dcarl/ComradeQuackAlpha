using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTurretControllerBeta : TurretControllerBeta
{
    protected bool placed;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;
        SetTransparent();
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

        ActivateTurret();       // For now
    }

    public void ActivateTurret()
    {
        alive = true;
        currentHealth = maxHealth;
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
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Standard");
            // Color oldColor = r.material.color;
            // r.material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
        }
    }

    public override void Die()
    {
        alive = false;
        SetTransparent();
    }
}
