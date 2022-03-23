using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingTurretController : PlaceableTurretControllerBeta
{
    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private ParticleSystem impactParticles;

    [SerializeField]
    private TrailRenderer bulletTrail;
    
    public float height = 5f;
    protected Vector3 startPos;

    public override void GoToPlacementPos()
    {
        base.GoToPlacementPos();
        startPos = new Vector3(transform.position.x, height, transform.position.z);
        transform.position = startPos;
    }
    
    public override void Fire()
    {
        // gunTurretAnimator.Play("FeedShooter_BaseAnimation");
        //gunTurretMuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, targetRange.range))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                
                TrailRenderer trail = Instantiate(bulletTrail, gun.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, hit.collider));
            }

        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hitPoint;
        // Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));

        if (!hitCollider.IsDestroyed())
        {
            EnemyControllerBeta target = hitCollider.GetComponent<EnemyControllerBeta>();

            if (target != null)
            {
                target.TakeDamage(damage);
                target.KnockBack(hitPoint, knockback);
            }
        }
        
        Destroy(trail.gameObject, trail.time);
    }
}
