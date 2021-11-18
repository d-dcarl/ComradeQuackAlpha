using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFist : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 moveDirection;

    void Start()
    {

        rb = GetComponent<Rigidbody>();

    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Test");
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Test2");
            moveDirection = rb.transform.position - other.transform.position;
            rb.AddForce(moveDirection.normalized * -100000f);
            //rb.AddForce(transform.forward * -10000000);
        }
    }
}
