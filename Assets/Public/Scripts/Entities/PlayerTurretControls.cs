using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretControls : MonoBehaviour
{
    //Public
    [SerializeField] public GameObject placeableTurret;
    [SerializeField] public GameObject placementTurret;
    [SerializeField] public float placementCooldown = 5.0f;
    [SerializeField] public float placementPreviewCooldown = 0.7f;
    [SerializeField] public float placementDistance = 2.0f;
    [SerializeField] public int turretCost = 3;

    public float turnSmoothTime = 0.1f;


    //Private
    private float cooldown = 0;
    private float previewCooldown;
    private bool isInPreview = false;
    private GameObject placeholder;
    private bool canPlace = true;
    private bool previewRed = false;

    // Start is called before the first frame update
    void Start()
    {
        previewCooldown = placementPreviewCooldown;
        placeholder = new GameObject();
    }
    private void Update()
    {
        //Input for placing a turret
        if (Input.GetKey(KeyCode.E))
        {
            if (ScoreManager.instance == null || cooldown <= 0 && ScoreManager.instance.score > turretCost)
            {

                if (!isInPreview)
                {
                    //Instantiate the placement turret
                    placeholder = Instantiate<GameObject>(placementTurret, this.transform);
                }
                isInPreview = true;

                //wait a minimum time before accepting inputs
                if (previewCooldown <= 0/* && canPlace*/)
                {
                    //delete the placement turret
                    Destroy(placeholder);

                    //instantiate the actual turret
                    Vector3 newPosition = new Vector3(transform.position.x + (placementDistance * this.transform.forward.x), transform.position.y, transform.position.z + (placementDistance * this.transform.forward.z));
                    Instantiate<GameObject>(placeableTurret, newPosition, this.transform.rotation);
                    
                    if(ScoreManager.instance != null)
                    {
                        ScoreManager.instance.score -= turretCost;
                    }

                    //set the cooldown for placing a turret
                    cooldown = placementCooldown;
                    previewCooldown = placementPreviewCooldown;
                    isInPreview = false;
                }

            }

        }

    }

    void FixedUpdate()
    {
        //update placement cooldown
        if (cooldown < 0)
        {
            cooldown = 0;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
        //update preview cooldown
        if (previewCooldown < 0)
        {
            previewCooldown = 0;
        }
        else if (isInPreview)
        {
            previewCooldown -= Time.deltaTime;
        }
        //update placment turret color if !canPlace
        if(!canPlace)
        {
            var renderer = placementTurret.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.red);
            previewRed = true;

        }
        if(canPlace && previewRed)
        {
            var renderer = placementTurret.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.blue);
            previewRed = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Turret")
        {
            canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Turret" && !CheckRadiusForTurret(placementTurret.transform.position, placementTurret.GetComponent<SphereCollider>().radius))
        {
            canPlace = true;
        }
    }

    private bool CheckRadiusForTurret(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        Collider nearest = new Collider();
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Turret")
            {
                return true;
            }


        }
        if (nearest == null)
        {
            return false;
        }
        return true;
    }
}
