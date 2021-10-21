using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    [SerializeField] private GameObject Projectile = null;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 velocity = new Vector3();
    [SerializeField] public float fireRate = 1;
     private float firingTimer;
    // Start is called before the first frame update
    void Start()
    {
        firingTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(firingTimer);
        if(firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
        if(firingTimer < 0)
        {
           firingTimer = 0;
        }
    }

    public void SpawnProjectile()
    {
        if(firingTimer == 0)
        {
            Debug.Log("Instantiation");
            var projectile = Instantiate(Projectile, spawnPoint.position, Quaternion.LookRotation(spawnPoint.transform.forward));
            if (projectile.TryGetComponent(out Rigidbody body))
            {
                
                body.velocity = body.transform.TransformDirection(velocity);
                Debug.Log("velocity:" + velocity);
            }
            firingTimer = fireRate;
        }

    }
}
