using System.Collections;
using System.Collections.Generic;
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
                TrailRenderer trail = Instantiate(bulletTrail, gun.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        // Instantiate(impactParticles, hit.point, Quaternion.LookRotation(hit.normal));

        EnemyControllerBeta target = hit.collider.GetComponent<EnemyControllerBeta>();

        if (target != null)
        {
            target.TakeDamage(damage);
            target.KnockBack(hit.point, knockback);
        }
        
        Destroy(trail.gameObject, trail.time);
    }
}
