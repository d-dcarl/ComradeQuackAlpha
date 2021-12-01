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
    }
}
