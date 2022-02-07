using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeControllerBeta : StructureControllerBeta
{
    /*
     * TODO: 
     * - change hitbox layers to "IgnoreRaycast"
     * - 
     */

    //public GameObject barricade;
    protected bool placed;
    protected BoxCollider hitBox;

    public override void Start()
    {
        base.Start();
        placed = false;
        alive = false;

        /* // TODO: ADD BOX COLLIDER
        hitBox = GetComponent<BoxCollider>();
        if (hitBox == null)
        {
            Debug.LogError("Error: Turrets need a box collider");
        } */
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
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, 0f, playerPos.z) + player.transform.forward * player.placementDistance;
            
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
        //hitBox.enabled = true;
        currentHealth = 0;
        //healthBarSlider.value = 0;
        ActivateBarricade();
    }

    public void ActivateBarricade()
    {
        //update our values
        alive = true;
        //update health
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
