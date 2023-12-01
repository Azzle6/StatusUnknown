namespace Weapon
{
    using Core.VariablesSO.VariableTypes;
    using DG.Tweening;
    using UnityEngine;
    
    public abstract class RangedWeapon : Weapon
    {
        [Header("Ads angle")]
        public Transform adsRotTr;
        public float adsAimAngle;
        public float adsRestAngle;
        public FloatVariableSO currentAmmo;

    
        public override void AimWithCurrentWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsAimAngle,0,0), 0.1f);
        }
    
        public override void RestWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsRestAngle,0,0), 0.1f);
        }
        
        public abstract float GetMagazineSize();

        public abstract void InitPool();

    }
}


