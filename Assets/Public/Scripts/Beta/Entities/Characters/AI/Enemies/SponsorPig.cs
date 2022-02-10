using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SponsorPig : EnemyControllerBeta
{
    public HitboxControllerBeta effectsHitBox;
    public float healthPerSecond;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        HealAlly();
    }

    void HealAlly()
    {
        if (effectsHitBox.tracked.Count > 0)
        {
            foreach(GameObject g in effectsHitBox.tracked)
            {
                if(g != null)
                {
                    if (g.GetComponent<EnemyControllerBeta>())
                    {
                        g.GetComponent<EntityControllerBeta>().Heal(healthPerSecond * Time.deltaTime);
                    }
                }
            }
        }
    }
}
