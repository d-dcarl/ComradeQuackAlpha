using System.Collections;
using System.Collections.Generic;
using Public.Scripts.Beta.Projectiles;
using UnityEngine;

public class MissileTurretController : PlaceableTurretControllerBeta
{
    public float splashRadius;
    public float retargetDistance;

    [SerializeField]
    private ParticleSystem smokePoof;
    
    public override void Start()
    {
        base.Start();
        
        
    }

    public override void Fire()
    {
        smokePoof.Play();
        
        MissileControllerBeta missile = Instantiate(Projectile).GetComponent<MissileControllerBeta>();
        missile.damage = damage;
        missile.knockback = knockback;
        missile.splashRadius = splashRadius;
        missile.retargetDistance = retargetDistance;
        missile.transform.position = gun.transform.position;
        missile.target = ClosestInRange();
        missile.transform.LookAt(missile.target.transform);
    }

    protected override void SetUpgrade(TowerUpgrade upgrade)
    {
        base.SetUpgrade(upgrade);
        var missileTurretUpgrade = upgrade as MissileTurretUpgrade;
        if (missileTurretUpgrade != null)
        {
            splashRadius = missileTurretUpgrade.splashRadius;
            retargetDistance = missileTurretUpgrade.retargetDistance;
        }
    }
}
