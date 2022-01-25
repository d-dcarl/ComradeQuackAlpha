using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTurretController : PlaceableTurretControllerBeta
{
    public float height = 5f;
    protected Vector3 startPos;

    public override void GoToPlacementPos()
    {
        base.GoToPlacementPos();
        startPos = new Vector3(transform.position.x, height, transform.position.z);
        transform.position = startPos;
    }
}
