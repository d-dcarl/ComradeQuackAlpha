using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerBeta : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootDelay;
    protected float shootTimer;

    public virtual void Start()
    {
        shootTimer = shootDelay;
    }

    public virtual void Update()
    {
        if(shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    public virtual void Shoot()
    {
        if(shootTimer <= 0f)
        {
            shootTimer = shootDelay;

            BulletControllerBeta bcb = Instantiate(bulletPrefab).GetComponent<BulletControllerBeta>();
            if (bcb == null)
            {
                Debug.LogError("Error: Bullet prefab must have a bullet controller beta script");
            }

            bcb.transform.position = transform.position;
            bcb.direction = transform.forward;
        }
    }
}
