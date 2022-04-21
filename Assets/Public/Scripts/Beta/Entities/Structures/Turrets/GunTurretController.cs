using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GunTurretController : PlaceableTurretControllerBeta
{
    public Animator gunTurretAnimator;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private ParticleSystem impactParticles;

    [SerializeField]
    private TrailRenderer bulletTrail;

    private float bulletSpeed = 100f;

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
        // gunTurretAnimator.Play("FeedShooter_BaseAnimation");
        //gunTurretMuzzleFlash.Play();
        RaycastHit hit;
        bool didHit = Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, targetRange.range, LayerMask.GetMask("Enemy"));
        
        if (didHit)
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
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            trail.transform.position = Vector3.LerpUnclamped(startPosition, hitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * bulletSpeed;

            yield return null;
        }

        trail.transform.position = hitPoint;

        if (!hitCollider.IsDestroyed())
        {
            EnemyControllerBeta target = hitCollider.GetComponent<EnemyControllerBeta>();

            if (target != null)
            {
                target.TakeDamage(damage);
                target.KnockBack(hitPoint, knockback);
                Instantiate(impactParticles, hitPoint, Quaternion.LookRotation(hitNormal));
            }
        }
        
        Destroy(trail.gameObject, trail.time);
    }
}
