namespace Weapon
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "TwinBlade", menuName = "CustomAssets/WeaponStat/TwinBlade", order = 1)]
    public class TwinBladeStat : WeaponStat
    {
        [Tooltip("The total of CastTime, BuildUpTime, ActiveTime, RecoveryTime, must match the animation length")]
        public MeleeAttack[] attacks;
        public float dotDamage;
        public float dotDuration;
        public float dotTickRate;
    }

}
