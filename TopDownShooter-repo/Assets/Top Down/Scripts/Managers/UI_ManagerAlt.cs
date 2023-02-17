using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UI_ManagerAlt : MonoBehaviour
{
    public GameManagerAlt gameManager;

    //public List<UI_WeaponSlot> uiWeaponSlots = new List<UI_WeaponSlot>();
    public UI_StatsManager uiStatsManager;

    private void Update()
    {
        //Bad 
        // Update Weapon Slots      
        /*
        foreach (WeaponAlt weap in gameManager.GetPlayer().GetWeapons())
        {
            if (weap.weaponType == WeaponType.Melee)
            {
                uiWeaponSlots[0].SetCoolDownTime(weap.fireRate, weap.fireTimer);
                uiWeaponSlots[0].SetIcon(weap.icon);
            }
            else if (weap.weaponType == WeaponType.Ranged)
            {
                uiWeaponSlots[1].SetCoolDownTime(weap.fireRate, weap.fireTimer);
                uiWeaponSlots[1].SetIcon(weap.icon);
            }
            else if (weap.weaponType == WeaponType.Defensive)
            {
                uiWeaponSlots[2].SetCoolDownTime(weap.fireRate, weap.fireTimer);
                uiWeaponSlots[2].SetIcon(weap.icon);
            }
        }
        */

        //update Stats
        if (uiStatsManager != null)
        {
            uiStatsManager.UpdateHealthBarAmount(gameManager.GetPlayer().maxHealth, gameManager.GetPlayer().GetHealth());
            //uiStatsManager.UpdateExpBarAmount(gameManager.progressionManager.GetXPIntoCurrentLevel(), gameManager.progressionManager.GetXPForLevel(gameManager.progressionManager.GetCurrentLevel()));
            //uiStatsManager.UpdateLevelText(gameManager.progressionManager.GetCurrentLevel());
            //uiStatsManager.UpdateExpText(gameManager.progressionManager.GetXPIntoCurrentLevel());
        }
    }
}
