using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comradDuckController : MonoBehaviour
{
    public PondController pondParent;

    //is the comrad in guard mode.
    public bool isTurret = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO go to place its supposed to be, then lock it
    }

    private void OnDestroy()
    {
        pondParent.duckIsDestoryed();
    }

}
