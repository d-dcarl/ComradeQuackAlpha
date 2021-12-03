using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingStoneControllerBeta : MonoBehaviour
{
    protected SteppingStoneControllerBeta next;
    protected SteppingStoneControllerBeta previous;

    public void SetNext(SteppingStoneControllerBeta sscb)
    {
        next = sscb;
    }

    public void SetPrevious(SteppingStoneControllerBeta sscb)
    {
        previous = sscb;
    }
}
