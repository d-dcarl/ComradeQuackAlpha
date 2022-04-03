using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeControllerBeta : StructureControllerBeta
{
    /*
     * TODO: 
     * - change hitbox layers to "IgnoreRaycast"
     * - add health bar to prefab
     * - replace placeholder model
     */

    protected bool placed;
    protected BoxCollider hitBox;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;

         // TODO: ADD BOX COLLIDER?
        hitBox = GetComponent<BoxCollider>();
        if (hitBox == null)
        {
            Debug.LogError("Error: Turrets need a box collider");
        }

        hitBox.isTrigger = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!placed)
        {
            GoToPlacementPos();
            cantDie = true;
        }
        else if (alive)
        {
            base.Update();
            cantDie = false;
        }
    }

    private void GoToPlacementPos()
    {
        if (GameManagerBeta.Instance != null)
        {
            PlayerControllerBeta player = GameManagerBeta.Instance.player;
            Vector3 placementPos = player.transform.position + player.transform.forward * player.placementDistance;

            if (Physics.Raycast(new Vector3(placementPos.x, 10f, placementPos.z), Vector3.down, out var hit, 100f, LayerMask.GetMask("Ground")))
            {
                transform.position = new Vector3(placementPos.x, hit.point.y, placementPos.z);
            }
            else
            {
                transform.position = new Vector3(placementPos.x, 0f, placementPos.z);
            }
            
            // Fix rotation
            Quaternion rotationAdjust = Quaternion.Euler(0, 90, 0);
            transform.rotation = player.transform.rotation;
            transform.Rotate(0, 90, 0);
            //transform.localScale = new Vector3(1, 5, 5);
        }
    }

    public void PlaceBarricade()
    {
        placed = true;
        hitBox.isTrigger = false;
        currentHealth = 0;
        //healthBarSlider.value = 0;
        ActivateBarricade();
    }

    public void ActivateBarricade()
    {
        //update our values
        alive = true;
        currentHealth = maxHealth;
        //healthBarSlider.value = maxHealth;

        //make the color the activated color
        SetOpaque();
    }

    public void SetOpaque()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Universal Render Pipeline/Lit");
        }
        gameObject.layer = LayerMask.NameToLayer("Player Structure");
    }
}
