using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHitboxControllerBeta : HitboxControllerBeta
{
    private float _range;
    // public float range
    // {
    //     get => _range;
    //     set
    //     {
    //         _range = value;
    //         sc.radius = _range;
    //     }
    // }

    public float range;

    protected SphereCollider sc;

    public override void Start()
    {
        base.Start();
        sc = GetComponent<SphereCollider>();
        if(sc == null)
        {
            Debug.LogError("Error: Range hitbox doesn't have a sphere collider.");
        } else
        {
            sc.radius = range;
        }
    }
}
