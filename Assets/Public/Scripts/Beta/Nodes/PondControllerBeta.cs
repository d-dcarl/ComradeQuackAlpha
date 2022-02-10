using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondControllerBeta : NodeControllerBeta
{
    public SpawnerControllerBeta spawner;
    public StructureControllerBeta structure;

    public GameObject nestPrefab;
    public GameObject towerPrefab;
    public GameObject styPrefab;

    public bool isSty;

    public MeshRenderer waterMesh;
    public Material waterTexture;
    public Material mudTexture;

    public override void Update()
    {
        base.Update();
        CheckCaptured();
    }

    public void CheckCaptured()
    {
        if(spawner == null)
        {
            ToggleSty();
        }
    }

    public void ToggleSty()
    {
        if (isSty)
        {
            MakePond();
        }
        else
        {
            MakeSty();
        }
    }

    public void MakeSty()
    {
        Destroy(structure);
        Destroy(spawner);
        spawner = GameObject.Instantiate(styPrefab, transform.position, Quaternion.identity).GetComponent<SpawnerControllerBeta>();

        waterMesh.material = mudTexture;
        isSty = true;
    }

    public void MakePond()
    {
        Destroy(spawner);

        structure = GameObject.Instantiate(towerPrefab, transform.position, Quaternion.identity).GetComponent<StructureControllerBeta>();
        spawner = GameObject.Instantiate(nestPrefab, transform.position, Quaternion.identity).GetComponent<SpawnerControllerBeta>();

        waterMesh.material = waterTexture;
        isSty = false;
    }
}
