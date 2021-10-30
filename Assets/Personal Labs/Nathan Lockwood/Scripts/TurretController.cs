using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform CurrentTarget = null;

    private Collider Other;
    
    public float rotationSpeed;
    public Quaternion initialRotation;

    private bool resetting = true;
    private Quaternion targetRotation;

    //can public see but only set here
    public bool HasTarget { get; private set; }

    
    // Start is called before the first frame update
    void Start()
    {
        HasTarget = false;
        initialRotation = transform.rotation;
        //rotationSpeed = 10.0f;
        
    }

    private void Update()
    {
        // Checks to see if the other collider being targeted is destroyed
       if (HasTarget && !Other)
        {
            HasTarget = false;
            resetting = true;
        }
        if(!HasTarget)
        {
            CurrentTarget = FindTarget(transform.position, 6.0f);
            if (CurrentTarget != transform)
            {
                HasTarget = true;
                resetting = false;
            }
        }

    }

    private void LateUpdate()
    {
        AimAtTarget();
        if(HasTarget)
        {
            if(TryGetComponent<LaunchProjectile>(out LaunchProjectile p))
            {
                p.SpawnProjectile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(HasTarget)
        {
            //we return if we want the turret to focus on an enemy until they die or leave range
            return;
        }
        if(!other.TryGetComponent(out Transform target))
        {
            HasTarget = false;
            resetting = true;
            return;
        }
        if (other.tag != "Enemy")
        {
            return;
        }

        Other = other;
        CurrentTarget = target;
        HasTarget = true;
        resetting = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasTarget)
        {
            return;
        }
        if (!other.TryGetComponent(out Transform target))
        {
            return;
        }
        if(CurrentTarget != target)
        {
            return;
        }

        Other = null;
        CurrentTarget = null;
        HasTarget = false;
        resetting = true;
    }

    private void AimAtTarget()
    {
        if(CurrentTarget == null)
        {
            if(!resetting)
            {
                return;
            }
            targetRotation = initialRotation;
        }
        else
        {
            targetRotation = resetting ? initialRotation : Quaternion.LookRotation(CurrentTarget.position - transform.position);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private Transform FindTarget(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        Debug.Log(hitColliders.Length);
        float smallest_dist = radius;
        Collider nearest = new Collider();
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Enemy")
            {
                //Place logic here for different targeting
                //default is closest
                float dist = Vector3.Distance(hitCollider.transform.position, transform.position);
                if (dist < smallest_dist)
                {
                    smallest_dist = dist;
                    nearest = hitCollider;
                }
            }
            

        }
        if(nearest == null)
        {
            return this.transform;
        }
        return nearest.transform;
    }
}
