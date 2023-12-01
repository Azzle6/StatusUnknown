namespace Weapon
{
    using Player;
    using UnityEngine;
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponManager weaponManager;
        public PlayerStat playerStat;
        public Sprite weaponSprite;
        public WeaponType weaponType;
        
        
        public abstract void ActionPressed();
    
        public abstract void ActionReleased();

        public abstract void Switched(Animator playerAnimator, bool OnOff);
        
        public abstract void Reload(Animator playerAnimator);

        public abstract void AimWithCurrentWeapon();
        
        public abstract void RestWeapon();

        

        public abstract void Hit();

        
    }

}

public enum WeaponType
{
    MELEE,
    RANGED
}