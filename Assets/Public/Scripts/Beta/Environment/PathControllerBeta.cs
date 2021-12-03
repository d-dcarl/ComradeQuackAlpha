using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathControllerBeta : MonoBehaviour
{
    public PondControllerBeta source;
    public PondControllerBeta dest;

    public List<SteppingStoneControllerBeta> steppingStones;     // In order from source to destination

    void Start()
    {
        LinkSteppingStones();
    }

    void LinkSteppingStones()
    {
        if(steppingStones.Count > 1)
        {
            for (int i = 0; i < steppingStones.Count - 1; i++)
            {
                steppingStones[i].SetNext(steppingStones[i + 1]);
            }

            for (int j = steppingStones.Count - 1; j > 0; j--)
            {
                steppingStones[j].SetPrevious(steppingStones[j - 1]);
            }
        }
        
    }
}