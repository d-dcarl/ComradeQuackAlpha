using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyControllerBeta : SpawnerControllerBeta
{
    public static List<StyControllerBeta> allStys;

    public override void Start()
    {
        base.Start();
        RegisterSty();
    }

    public void RegisterSty()
    {
        if (allStys == null)
        {
            allStys = new List<StyControllerBeta>();
        }
        allStys.Add(this);
    }
}
