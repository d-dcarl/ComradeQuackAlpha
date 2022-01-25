using System.Collections;
using System.Collections.Generic;
using Public.Scripts.Beta.Projectiles;
using UnityEngine;

public class MissileTurretController : PlaceableTurretControllerBeta
{
    public override void Start()
    {
        base.Start();
    }

    public override void Fire()
    {
        MissileControllerBeta missile = Instantiate(Projectile).GetComponent<MissileControllerBeta>();
        missile.transform.position = gun.transform.position;
        missile.target = ClosestInRange();
        missile.transform.LookAt(missile.target.transform);
    }
}
