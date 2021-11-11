using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHitboxController : HitboxController
{
    private SphereCollider sc;

    public override void Start()
    {
        base.Start();
        sc = GetComponent<SphereCollider>();
    }

    public void setRange(float newRange)
    {
        sc.radius = newRange;
    }
}
