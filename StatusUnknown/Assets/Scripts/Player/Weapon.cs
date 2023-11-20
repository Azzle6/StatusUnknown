namespace Player
{
    using UnityEngine;
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponManager weaponManager;
        public PlayerStat playerStat;
        
        
        public abstract void ActionPressed();
    
        public abstract void ActionReleased();

        public abstract void Switched(Animator playerAnimator, bool OnOff);
        

        public abstract void Hit();

        
    }

}

