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
        alive = true;
        SetTransparent();
    }

    public override void Update()
    {
        if(!placed)
        {
            GoToPlacementPos();
            if(Input.GetMouseButtonDown(1) && ValidPlacementLocation())
            {
                PlaceTurret();
            }
        }
        else if(alive)
        {
            base.Update();
        }
    }

    public bool ValidPlacementLocation()
    {
        return true;
    }

    void GoToPlacementPos()
    {
        if(GameManagerBeta.Instance != null)
        {
            Vector3 playerPos = GameManagerBeta.Instance.player.transform.position;
            transform.position = new Vector3(playerPos.x, 0f, playerPos.z);
        }
    }

    public void PlaceTurret()
    {
        placed = true;

        hitBox.enabled = true;
        SetOpaque();
    }

    public void ActivateTurret()
    {
        alive = true;
        currentHealth = maxHealth;
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
