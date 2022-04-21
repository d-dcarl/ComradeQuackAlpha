using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    private PlayerControllerBeta playerController;

    [SerializeField]
    private Transform bulletSpawn;
    
    [SerializeField]
    private ParticleSystem impactParticles;

    [SerializeField]
    private TrailRenderer bulletTrail;

    [SerializeField]
    private float bulletSpeed = 100f;

    [SerializeField]
    private LayerMask bulletRayCastMask;

    [SerializeField]
    private float damage = 5;

    [SerializeField]
    private float knockback = 5;

    public virtual void Start()
    {
        shootTimer = shootDelay;
        crosshair = GameObject.Find("Crosshair");
        gunAnimator = GetComponentInChildren<Animator>();
        gunMuzzleFlash = GetComponentInChildren<VisualEffect>();
        crosshair.SetActive(false);
        zoomedIn = false;
        PlayerLayerMask = LayerMask.GetMask("Player", "UI", "Ignore Raycast");

        //get the player controller so we stop when paused
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerControllerBeta>();
    }

    public virtual void Update()
    {
        //if the game is paused do none of this
        if(playerController.paused)
        {
            return;
        }

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
            shootTimer = shootDelay;
            
            gunAnimator.Play("PistolAnimation");
            gunMuzzleFlash.Play();

            Vector3 direction = zoomedIn ? GetDirection() : transform.forward;
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);

            if (Physics.Raycast(bulletSpawn.position, direction, out RaycastHit hit, float.MaxValue, bulletRayCastMask))
            {
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, hit.collider, true));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, direction * 100, Vector3.zero, null, false));
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            trail.transform.position = Vector3.LerpUnclamped(startPosition, hitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * bulletSpeed;

            yield return null;
        }

        trail.transform.position = hitPoint;

        if (madeImpact)
        {
            Instantiate(impactParticles, hitPoint, Quaternion.LookRotation(hitNormal));
            
            if (!hitCollider.IsDestroyed())
            {
                if (hitCollider.gameObject.CompareTag("Enemy") || hitCollider.gameObject.CompareTag("Enemy Structure"))
                {
                    EnemyControllerBeta target = hitCollider.GetComponent<EnemyControllerBeta>();

                    if (target != null)
                    {
                        target.TakeDamage(damage);
                        target.KnockBack(hitPoint, knockback);
                        Instantiate(impactParticles, hitPoint, Quaternion.LookRotation(hitNormal));
                    }
                }
            }
        }
        
        Destroy(trail.gameObject, trail.time);
    }

    void ShowCrosshair()
    {
        if (crosshair != null)
        {
            crosshair.SetActive(true);
            //crosshair.transform.position = Input.mousePosition;
            crosshair.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);     // lock crosshair to centerscreen
        }
        else
        {
            Debug.Log("Crosshair has not been set in the scene.");
        }

    }

    private Vector3 GetDirection()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
        RaycastHit hit;
        Vector3 result = ray.direction;

        if (Physics.Raycast(ray, out hit, 100/*, ~PlayerLayerMask*/))
        {
            var heading = hit.point - transform.position;
            var distance = heading.magnitude;
            result = heading / distance; // This is now the normalized direction.

            //Debug.Log(hit.collider.gameObject.layer);
        }

        // TESTING CODE ------------------------------------------------------------------
        /*PlayerControllerBeta bad = hit.transform.GetComponent<PlayerControllerBeta>();
        if (bad != null)
            Debug.Log("hitting player"); */
        
        // BUG: 4/10/22 - random shooting bug again, hitting layer 0? 15ish feet from first pigsty
        // -------------------------------------------------------------------------------

        return result;
    }
}
