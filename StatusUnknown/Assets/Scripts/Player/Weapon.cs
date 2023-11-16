namespace Player
{
    using UnityEngine;
    public abstract class Weapon : MonoBehaviour
    {
        public WeaponManager weaponManager;
        public bool meleeWeapon;
        [Header("Ads angle")]
        public Transform adsRotTr;
        public float adsAimAngle;
        public float adsRestAngle;
        
        public abstract void ActionPressed();
    
        public abstract void ActionReleased();
    
        public abstract void Reload(Animator playerAnimator);

        public abstract void Switched(Animator playerAnimator, bool OnOff);

        public abstract void Hit();

        
    }

}

