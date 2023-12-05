
using Input;

namespace Weapon
{
    using Core.Pooler;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PhotonPistol", menuName = "CustomAssets/WeaponStat/PhotonPistol", order = 1)]
    public class PhotonPistolStat : WeaponStat
    {
        [Header("Damage")]
        public float maxDamage;
        public AnimationCurve damageCurve;
        [Header("Projectile")]
        public AnimationCurve projectileSize;
        public float maxProjectileSize;
        public float projectileSpeed; 
        [Header("Charge")]
        public float maxTimeCharge;
        [Header("Rumble")] 
        public GamePadRumbleWithTimer rumbleScaling;
        public GamePadRumbleWithTimer rumbleOnShoot;
    }
}

