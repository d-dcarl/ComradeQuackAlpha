using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControllerBeta : StructureControllerBeta
{
    public GameObject head;
    public GameObject gun;

    public RangeHitboxControllerBeta targetRange;
    public List<string> canShoot;

    public bool predictiveAiming;

    public float rotationSpeed;
    [HideInInspector]
    public Quaternion initialRotation;
    private Quaternion targetRotation;

    [SerializeField] private GameObject Projectile;
    private float bulletSpeed;
    [SerializeField] public float fireRate = 1;
    private float firingTimer;

    public override void Start()
    {
        base.Start();
        firingTimer = fireRate;

        BulletControllerBeta bc = Projectile.GetComponent<BulletControllerBeta>();
        if (bc != null)
        {
            bulletSpeed = bc.speed;
            Debug.Log("Bullet Speed:" + bulletSpeed);
        } else
        {
            Debug.LogError("Error: Projectile does not have a BulletControllerBeta component");
        }
    }

    public override void Update()
    {
        base.Update();
        AimAtClosestTarget();
    }

    public virtual void AimAtClosestTarget()
    {
        GameObject target = ClosestInRange();
        if (target == null)
        {
            targetRotation = initialRotation;
        }
        else
        {
            RotateGun(target);
            Shoot();
        }
    }

    protected void RotateGun(GameObject target)
    {
        Vector3 projectedPos;
        if (predictiveAiming)
        {
            projectedPos = ShotTracking(target);     // Predict where it will be
        }
        else
        {
            projectedPos = target.transform.position;   // Shoot directly at target for now
        }

        targetRotation = Quaternion.LookRotation(projectedPos - transform.position);

        head.transform.rotation = Quaternion.Lerp(head.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 60f);
        head.transform.localEulerAngles = new Vector3(0f, head.transform.localEulerAngles.y, 0f);
        if (target != null)
            gun.transform.LookAt(target.transform);
        gun.transform.localEulerAngles = new Vector3(gun.transform.localEulerAngles.x, 0f, 0f);
    }

    protected Vector3 ShotTracking(GameObject target)
    {
        // Initial guess
        Vector3 projectedPos = target.transform.position;
        float timeExpected;

        // Iterate so your guesses get closer
        for (int i = 0; i < 3; i++)
        {

            // Update how long it would take to get to your current guess
            timeExpected = Vector3.Distance(projectedPos, gun.transform.position) / bulletSpeed;

            // See how far they would have moved by then
            projectedPos = target.transform.position + target.GetComponent<Rigidbody>().velocity * timeExpected;
        }
        

        return projectedPos;
    }

    public virtual void Shoot()
    {
        firingTimer -= Time.deltaTime;
        if (firingTimer <= 0f)
        {
            SpawnProjectile();
            firingTimer = fireRate;
        }
    }

    public virtual void SpawnProjectile()
    {
        BulletControllerBeta projectile = Instantiate(Projectile).GetComponent<BulletControllerBeta>();
        projectile.transform.position = gun.transform.position;
        projectile.direction = gun.transform.forward;
    }

    public GameObject ClosestInRange()
    {
        GameObject closest = null;
        foreach (GameObject g in targetRange.tracked)
        {
            if (g != null && canShoot.Contains(g.tag))
            {
                float dist = Vector3.Distance(transform.position, g.transform.position);
                if (closest == null || dist < Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = g;
                }
            }
        }
        return closest;
    }
}
