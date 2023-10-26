using System.Collections;
using System.Collections.Generic;
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

    public void ReloadLastEquipedWeapon()
    {
        weapons[currentWeaponIndex].Reload();
    }
}
