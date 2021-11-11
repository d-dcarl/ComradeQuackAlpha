using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRocket : MonoBehaviour
{
    [SerializeField] private GameObject Projectile = null;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public float fireRate = 1;
    [SerializeField] public float bulletLifetime = 5;
    private float firingTimer;
    [SerializeField] private Vector3 relativeVelocity = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        firingTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        //Reducde the cooldown of firing
        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }
        if (firingTimer < 0)
        {
            firingTimer = 0;
        }
    }

    public void SpawnProjectile()
    {
        if (firingTimer == 0)
        {
            //Debug.Log("Instantiation");
            var projectile = Instantiate(Projectile, spawnPoint.position, Quaternion.LookRotation(spawnPoint.transform.forward));
            if (projectile.TryGetComponent(out Rigidbody body))
            {

                body.velocity = transform.TransformDirection(relativeVelocity);
                //Debug.Log("velocity:" + relativeVelocity);
            }
            firingTimer = fireRate;
            Destroy(projectile, bulletLifetime);
        }

    }
}
