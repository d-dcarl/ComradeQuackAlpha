using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public GameObject player;

    public float speed = 1.0f;

    private bool moveLeft = false;
    private float startTime;
    private float distance;
    private float reverseDistance;
    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        
        // Calculate the journey length.
        distance = Vector3.Distance(start.position, end.position);
    }
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.

        if (transform.position == end.position)
        {
            moveLeft = true;
            startTime = Time.time;

        }
       if(transform.position == start.position)
        {
            moveLeft = false;
            startTime = Time.time;
        }
       if(moveLeft)
        {
            transform.position = Vector3.Lerp(end.position, start.position, distCovered);
            
        }
       if(!moveLeft)
        {
            transform.position = Vector3.Lerp(start.position, end.position, distCovered);
        }
    }
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().playerDeath();
            //other.gameObject.transform.position = ;
        }
    }
}
