namespace Weapon
{
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
    using Player;
    using UnityEngine;
    
    public abstract class RangedWeapon : Weapon
    {
        [Header("Ads angle")]
        public Transform adsRotTr;
        public float adsAimAngle;
        public float adsRestAngle;
    
        public virtual void AimWithCurrentWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsAimAngle,0,0), 0.1f);
        }
    
        public void RestWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsRestAngle,0,0), 0.1f);
        }
        
        public abstract void Reload(Animator playerAnimator);

    }
}


