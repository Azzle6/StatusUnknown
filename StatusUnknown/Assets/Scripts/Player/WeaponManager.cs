using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    private int currentWeaponIndex;
    
    private void Awake()
    {
        currentWeaponIndex = 0;
        
    }
    
    public void PressTriggerWeapon(int weaponNo)
    {
        currentWeaponIndex = weaponNo;
        weapons[currentWeaponIndex].TriggerPressed();
    }
    
    
    public void ReleaseTriggerWeapon()
    {
        weapons[currentWeaponIndex].TriggerReleased();
    }

    public void AimWithCurrentWeapon()
    {
        weapons[currentWeaponIndex].adsRotTr.DOLocalRotate(new Vector3(weapons[currentWeaponIndex].adsAngle,0,0), 0.1f);
    }
    
    public void RestWeapon()
    {
        weapons[currentWeaponIndex].adsRotTr.DOLocalRotate(new Vector3(weapons[currentWeaponIndex].adsRestAngle,0,0), 0.1f);

    }

    public void ReloadLastEquipedWeapon()
    {
        weapons[currentWeaponIndex].Reload();
    }
}
