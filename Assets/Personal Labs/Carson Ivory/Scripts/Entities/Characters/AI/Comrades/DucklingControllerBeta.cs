using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucklingControllerBeta : ComradeControllerBeta
{
    public override ProjectileControllerBeta Shoot()
    {
        BulletControllerBeta bcb = base.Shoot() as BulletControllerBeta;
        if(bcb != null)
            bcb.direction = transform.forward;
        return bcb;
    }
}
