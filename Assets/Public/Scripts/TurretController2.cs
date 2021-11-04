using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController2 : MonoBehaviour
{
    public float rotationSpeed;
    public Quaternion initialRotation;

    private bool resetting = true;
    private Quaternion targetRotation;

    private List<PigController> inRange;
    [HideInInspector]
    public GameObject target;

    [SerializeField] private GameObject Projectile = null;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public float fireRate = 1;
    private float firingTimer;

    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        inRange = new List<PigController>();
        target = null;
        initialRotation = transform.rotation;
        //rotationSpeed = 10.0f;

        firingTimer = fireRate;

    }

    private void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            inRange.Add(other.GetComponent<PigController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            inRange.Remove(other.GetComponent<PigController>());
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
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private GameObject ClosestInRange()
    {
        if(inRange.Count < 1)
        {
            return null;
        }

        float radius = GetComponent<SphereCollider>().radius;
        
        PigController nearest = null;
        float smallest_dist = radius;
        for (int i = 0; i < inRange.Count; i++)
        {
            PigController pig = inRange[i];
            if(pig != null)
            {
                float dist = Vector3.Distance(pig.transform.position, transform.position);
                if (dist < smallest_dist)
                {
                    smallest_dist = dist;
                    nearest = pig;
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
        projectile.transform.position = spawnPoint.position;
        projectile.direction = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        projectile.speed = bulletSpeed;

        firingTimer = fireRate;
    }
}
