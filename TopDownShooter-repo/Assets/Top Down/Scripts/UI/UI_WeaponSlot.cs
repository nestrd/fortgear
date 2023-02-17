using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponSlot : MonoBehaviour
{
    public Image coolDownImage;
    public Image icon;

   
    public void SetCoolDownTime(float fireRate, float fireTime)
    {
        coolDownImage.fillAmount = fireTime / fireRate;
    }
    public void SetIcon(Sprite T)
    {
        icon.sprite = T;
        icon.color = new Color(1, 1, 1, 1);
    }
}
