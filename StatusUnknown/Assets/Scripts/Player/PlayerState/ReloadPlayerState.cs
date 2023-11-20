namespace Player
{
using UnityEngine;
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


