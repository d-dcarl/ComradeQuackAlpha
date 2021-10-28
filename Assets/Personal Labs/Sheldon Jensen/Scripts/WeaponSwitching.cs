using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    //Reference: https://youtu.be/Dn_BUIVdAPg

    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        // iterate through weapons if leftShift was pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            selectedWeapon++;
            if(selectedWeapon > transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
