using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is abstract. Use a child matching the actual tower you want to make an upgrade for.
public class TowerUpgrade : ScriptableObject
{
    public float damage;
    public float knockback;
    public float range;
    public float fireRate;
}
