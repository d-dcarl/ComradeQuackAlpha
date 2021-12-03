using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerBeta : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootDelay;
    protected float shootTimer;
    public GameObject crosshair;
    public bool zoomedIn;

    public virtual void Start()
    {
        shootTimer = shootDelay;
        crosshair = GameObject.Find("Crosshair");
        crosshair.SetActive(false);
        zoomedIn = false;
    }

    public virtual void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ShowCrosshair();
            zoomedIn = true;

            // set cursor
            Cursor.lockState = CursorLockMode.Confined;
            
        }
        else
        {
            crosshair.SetActive(false);
            zoomedIn = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        Cursor.visible = false;
    }

    public virtual void Shoot()
    {
        if(shootTimer <= 0f)
        {
            if (zoomedIn)
            {
                // TODO: shoot at crosshair
                shootTimer = shootDelay;

                BulletControllerBeta bcb = Instantiate(bulletPrefab).GetComponent<BulletControllerBeta>();
                if (bcb == null)
                {
                    Debug.LogError("Error: Bullet prefab must have a bullet controller beta script");
                }

                bcb.transform.position = transform.position;
                bcb.direction = transform.forward;
            }
            else
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

    void ShowCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(true);
            crosshair.transform.position = Input.mousePosition;
        }
        else
        {
            Debug.Log("Crosshair has not been set in the scene.");
        }

    }
}
