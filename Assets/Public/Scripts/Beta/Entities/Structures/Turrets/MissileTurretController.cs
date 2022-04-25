using System.Collections;
using System.Collections.Generic;
using Public.Scripts.Beta.Projectiles;
using UnityEngine;

public class MissileTurretController : PlaceableTurretControllerBeta
{
    public float splashRadius;
    public float retargetDistance;

    [SerializeField]
    private ParticleSystem smokePoof;

    private int gunCount;
    private int gunIndex;
    private GameObject topGun;
    
    public override void Start()
    {
        base.Start();
        
        
    }

    protected override void SetModels()
    {
        base.SetModels();

        topGun = gun;
        gunCount = topGun.transform.childCount;
        if (gunCount > 0)
        {
            gunIndex = 0;
            gun = topGun.transform.GetChild(0).gameObject;
        }

        if (upgradeLevel >=1)
            turretBase.SetActive(false);
        else
            turretBase.SetActive(true);
    }

    public override void Fire()
    {
        smokePoof.transform.parent = head.transform;
        smokePoof.transform.position = gun.transform.position;
        smokePoof.transform.rotation = gun.transform.rotation;
        smokePoof.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/weapons/pistol/shoot", GetComponent<Transform>().position);

        MissileControllerBeta missile = Instantiate(Projectile).GetComponent<MissileControllerBeta>();
        missile.damage = damage;
        missile.knockback = knockback;
        missile.splashRadius = splashRadius;
        missile.retargetDistance = retargetDistance;
        missile.transform.position = gun.transform.position;
        missile.target = ClosestInRange();
        missile.transform.LookAt(missile.target.transform);

        if (gunCount > 0)
        {
            gunIndex = (gunIndex + 1) % gunCount;
            gun = topGun.transform.GetChild(gunIndex).gameObject;
        }
    }

    protected override void SetUpgrade(TowerUpgrade upgrade)
    {
        base.SetUpgrade(upgrade);
        var missileTurretUpgrade = upgrade as MissileTurretUpgrade;
        if (missileTurretUpgrade != null)
        {
            splashRadius = missileTurretUpgrade.splashRadius;
            retargetDistance = missileTurretUpgrade.retargetDistance;
        }
    }

    protected override void RotateGun(GameObject target)
    {
        base.RotateGun(target);
        head.transform.localEulerAngles = new Vector3(0f, head.transform.localEulerAngles.y, 0f);
        hitBox.transform.localEulerAngles = new Vector3(0f, hitBox.transform.localEulerAngles.y, 0f);
        duck.transform.localEulerAngles = new Vector3(0f, duck.transform.localEulerAngles.y, 0f);
    }
}
