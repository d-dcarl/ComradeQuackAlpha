using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTurretControllerBeta : TurretControllerBeta
{
    protected bool placed;
    protected bool manned;

    public override void Start()
    {
        base.Start();
        placed = false;
        manned = false;
        SetTransparent();
        SetOpaque();
    }

    public void SetTransparent()
    {
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
}
