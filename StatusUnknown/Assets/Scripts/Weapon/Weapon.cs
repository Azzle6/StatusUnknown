namespace Weapon
{
    using Inventory;
    using Module.Behaviours;
    using Player;
    using UnityEngine;
    using Weapons;

    public abstract class Weapon : MonoBehaviour
    {
        public WeaponManager weaponManager;
        public PlayerInventorySO inventory;
        public WeaponDefinitionSO weaponDefinition;
        public PlayerStat playerStat;
        public Sprite weaponSprite;
        public WeaponType weaponType;
        
        
        public virtual bool ActionPressed() { return false;} 
    
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