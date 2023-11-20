namespace Player
{
using UnityEngine;
    public class ReloadPlayerState : PlayerState
    {
        [SerializeField] private WeaponManager weaponManager;
        
        
        public override void OnStateEnter()
        {
            weaponManager.ReloadLastEquipedWeapon();
        }

        public override void OnStateExit()
        {
            
        }
    }
}


