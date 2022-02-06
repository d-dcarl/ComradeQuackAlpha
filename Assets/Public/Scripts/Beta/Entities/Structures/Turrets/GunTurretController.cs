using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurretController : PlaceableTurretControllerBeta
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, targetRange.range))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyControllerBeta target = hit.collider.GetComponent<EnemyControllerBeta>();

                if (target != null)
                {
                    target.TakeDamage(damage);
                    target.KnockBack(hit.point, knockback);
                }
                
                Debug.DrawLine(gun.transform.position, hit.point);
            }

        }
    }
}