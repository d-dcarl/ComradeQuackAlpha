using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform CurrentTarget = null;
    
    public float rotationSpeed;
    public Quaternion initialRotation;

    private bool resetting = true;
    private Quaternion targetRotation;
    private bool HasTarget;

    
    // Start is called before the first frame update
    void Start()
    {
        HasTarget = false;
        initialRotation = transform.rotation;
        rotationSpeed = 10.0f;
        
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
}
