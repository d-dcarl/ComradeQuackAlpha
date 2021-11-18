using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    public override void Die()
    {
        Debug.Log("Player is dead!");
    }
}
