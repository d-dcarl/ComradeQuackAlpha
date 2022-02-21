using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunControllerBeta : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootDelay;
    protected float shootTimer;
    public GameObject crosshair;
    public bool zoomedIn;
    public LayerMask PlayerLayerMask;
    private VisualEffect gunMuzzleFlash;
    private Animator gunAnimator;

    public virtual void Start()
    {
        shootTimer = shootDelay;
        crosshair = GameObject.Find("Crosshair");
        gunAnimator = GetComponentInChildren<Animator>();
        gunMuzzleFlash = GetComponentInChildren<VisualEffect>();
        crosshair.SetActive(false);
        zoomedIn = false;
        PlayerLayerMask = LayerMask.GetMask("Player", "UI", "Ignore Raycast");
    }

    public virtual void Update()
    {
        if (Input.GetMouseButton(1))
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
            gunAnimator.Play("PistolAnimation");
            gunMuzzleFlash.Play();
            if (zoomedIn)
            {
                shootTimer = shootDelay;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                BulletControllerBeta bcb = Instantiate(bulletPrefab).GetComponent<BulletControllerBeta>();
                if (bcb == null)
                {
                    Debug.LogError("Error: Bullet prefab must have a bullet controller beta script");
                }

                bcb.transform.position = transform.position;
                bcb.direction = GetDirection();
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

    private Vector3 GetDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 result = ray.direction;

        if (Physics.Raycast(ray, out hit, 100/*, ~PlayerLayerMask*/))
        {
            var heading = hit.point - transform.position;
            var distance = heading.magnitude;
            result = heading / distance; // This is now the normalized direction.

            Debug.Log(hit.collider.gameObject.layer);
        }

        // TESTING CODE ------------------------------------------------------------------
        /*PlayerControllerBeta bad = hit.transform.GetComponent<PlayerControllerBeta>();
        if (bad != null)
            Debug.Log("hitting player"); */
        
        // -------------------------------------------------------------------------------

        return result;
    }
}
