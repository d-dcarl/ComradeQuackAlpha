using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunTurretController : PlaceableTurretControllerBeta
{
    public Animator gunTurretAnimator;
    //public VisualEffect gunTurretMuzzleFlash;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        gunTurretAnimator = GetComponentInChildren<Animator>();
        //gunTurretMuzzleFlash = GetComponentInChildren<VisualEffect>();
    }

    public override void Fire()
    {
        gunTurretAnimator.Play("FeedShooter_BaseAnimation");
        //gunTurretMuzzleFlash.Play();
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
