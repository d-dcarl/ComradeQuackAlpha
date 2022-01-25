using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurretController : PlaceableTurretControllerBeta
{
    public float damage;
    public float knockback;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, targetRange.range))
        {
            EnemyControllerBeta target = hit.collider.GetComponent<EnemyControllerBeta>();

            if (target != null)
            {
                target.TakeDamage(damage);
                target.KnockBack(-hit.normal, knockback);
            }
        }
    }
}
