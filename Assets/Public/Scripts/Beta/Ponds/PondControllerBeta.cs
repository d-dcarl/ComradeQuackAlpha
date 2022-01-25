using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondControllerBeta : MonoBehaviour
{
    public SpawnerControllerBeta spawner;

    public List<PathControllerBeta> paths;

    [SerializeField] public GameObject pond;
    [SerializeField] public Material waterTexture;

    //boolean that tells whether this prefab was placed in the level
    public bool placedInLevel = false;

    //changes the pond's water texture to this pond's texture (IE water to mud and vice versa)
    public void changePondTexture()
    {
        //gets the water's mesh renderer, then
        pond.transform.GetChild(1).GetComponent<MeshRenderer>().material = waterTexture;
    }

    //gives the pond object associated with this pond (the water and shore)
    public void setPond(GameObject replacePond)
    {
        pond = replacePond;
        changePondTexture();
    }

}
