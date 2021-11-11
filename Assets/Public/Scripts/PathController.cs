using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public GameObject source;
    public GameObject dest;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 displacement = source.transform.position - dest.transform.position;
        float dist = displacement.magnitude;

        gameObject.transform.position = (source.transform.position + dest.transform.position) / 2;
        gameObject.transform.LookAt(dest.transform);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, dist);
    }
}
