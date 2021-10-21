using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Reference: https://youtu.be/THnivyG0Mvo

    public float damage = 10;
    public float range = 100;

    public GameObject gun;
    public GameObject impactEffect;
    public Vector3 gunRotation;
    public LayerMask PlayerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(gun.transform.rotation);
        //gunRotation = new Vector3(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2"))
        {
            Shoot();
        }

        //gun.transform.Rotate(90,0,0);
    }

    void Shoot()
    {
        /*RaycastHit hitInfo;
        //if (Physics.Raycast(gun.transform.position, gun.transform.rotation * gun.transform.forward, out hitInfo, range))
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
            //Debug.Log(gun.transform.rotation);

            // particle effect on hit
            Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }*/

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, range, ~PlayerLayerMask))
        {
            //Transform objectHit = hit.transform;
            Debug.Log(hit.transform.name);
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            // Do something with the object that was hit by the raycast.
        }
    }
}
