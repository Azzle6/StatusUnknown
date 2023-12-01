namespace Player
{
using UnityEngine;
using Weapon;
    public class ReloadPlayerState : PlayerState
    {
        [SerializeField] private WeaponManager weaponManager;
        
        
        public override void OnStateEnter()
        {
            weaponManager.ReloadWeapon();
        }

        public override void OnStateExit()
        {
            
        }
    }
}


