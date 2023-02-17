using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged,
    Defensive
}

public class WeaponSlotAlt : MonoBehaviour
{
    public WeaponType weaponType;
    private bool active = true;

    private void Awake()
    {
        WeaponAlt weapon = GetWeapon();

        if (weapon != null)
        {
            if(weapon.weaponType != weaponType)
            {
                Destroy(weapon.gameObject);
            }
        }
    }

    public void AttachWeapon(WeaponAlt weapon)
    {
        //If Weapon Slot
        if (active)
        {
            if (weapon.weaponType == weaponType)
            {
                WeaponAlt weap = GetWeapon();

                if (weap != null)
                {
                    Destroy(weap.gameObject);
                }

                weapon.transform.position = transform.position;
                weapon.transform.rotation = transform.rotation;
                weapon.transform.parent = transform;
            }            
        }
    }

    public WeaponAlt GetWeapon()
    {
        WeaponAlt weapon = GetComponentInChildren<WeaponAlt>();
        return weapon;
    }
}
