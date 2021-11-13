using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController2 : EntityController
{
    public GameObject head;
    public GameObject gun;

    public float range;
    public RangeHitboxController rangeCollider;

    public float rotationSpeed;
    [HideInInspector]
    public Quaternion initialRotation;

    private bool resetting = true;
    private Quaternion targetRotation;

    [HideInInspector]
    public GameObject target;

    [SerializeField] private GameObject Projectile = null;
    [SerializeField] public float fireRate = 1;
    private float firingTimer;

    public float bulletSpeed;

    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            InitializeTurret();
        }
    }

    public void InitializeTurret()
    {
        target = null;
        initialRotation = transform.rotation;
        //rotationSpeed = 10.0f;

        firingTimer = fireRate;
        if (GameManager.Instance.turrets == null)
        {
            GameManager.Instance.turrets = new List<TurretController2>();
        }
        GameManager.Instance.turrets.Add(this);
        initialized = true;
    }

    private void Update()
    {
        if(!initialized)
        {
            InitializeTurret();
        }
        target = ClosestInRange();
        if(target != null)
        {
            AimAtTarget();

            firingTimer -= Time.deltaTime;
            if (firingTimer < 0)
            {
                firingTimer = fireRate;
                SpawnProjectile();
            }
            
        }
    }

    private void AimAtTarget()
    {
        if (target == null)
        {
            targetRotation = initialRotation;
        }
        else
        {
            float timeExpected = Vector3.Distance(target.transform.position, transform.position) / bulletSpeed;
            Vector3 projectedPos = target.transform.position + target.GetComponent<Rigidbody>().velocity * timeExpected;
            targetRotation = Quaternion.LookRotation(projectedPos - transform.position);

            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            head.transform.localEulerAngles = new Vector3(0f, head.transform.localEulerAngles.y, 0f);
            gun.transform.LookAt(target.transform);
            gun.transform.localEulerAngles = new Vector3(gun.transform.localEulerAngles.x, 0f, 0f);
        }
        
    }

    private GameObject ClosestInRange()
    {
        if(rangeCollider.tracked.Count < 1)
        {
            return null;
        }
        
        PigController nearest = null;
        float smallestDist = -1f;
        for (int i = 0; i < rangeCollider.tracked.Count; i++)
        {
            if(rangeCollider.tracked[i] != null)
            {
                PigController pig = rangeCollider.tracked[i].GetComponent<PigController>();
                if (pig != null)
                {
                    float dist = Vector3.Distance(pig.transform.position, transform.position);
                    if (smallestDist < 0f || dist < smallestDist)
                    {
                        smallestDist = dist;
                        nearest = pig;
                    }
                }
            }
            
        }
        if (nearest == null)
        {
            return null;
        }
        return nearest.gameObject;
    }

    public void SpawnProjectile()
    {
        BulletController2 projectile = Instantiate(Projectile).GetComponent<BulletController2>();
        projectile.transform.position = gun.transform.position;
        projectile.direction = gun.transform.forward;
        projectile.speed = bulletSpeed;

        firingTimer = fireRate;
    }
}
