using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretDisplay : MonoBehaviour
{
    public PlayerControllerBeta player;
    public List<Sprite> turretSprites;

    public Image lastIcon;
    public Image CurrentTurretIcon;
    public Image nextIcon;

    public void UpdateSprites()
    {
        int currentTurret = player.GetTurretIndex();
        int last = currentTurret - 1;
        int next = currentTurret + 1;

        if(last < 0)
        {
            last = turretSprites.Count - 1;
        }
        if(next >= turretSprites.Count)
        {
            next = 0;
        }

        CurrentTurretIcon.sprite = turretSprites[currentTurret];

        lastIcon.sprite = turretSprites[last];
        nextIcon.sprite = turretSprites[next];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprites();
    }
}
