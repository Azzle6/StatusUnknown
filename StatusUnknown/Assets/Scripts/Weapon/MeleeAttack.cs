namespace Weapon
{
    using System;
    using UnityEngine;
    using Input;
    using UnityEngine.Serialization;

    [Serializable]
    public class MeleeAttack
    {
        [Header("Attack Stats")]
        public float attackDamage;
        public float attackLength;
        public float attackAngle;
        public float attackKnockback;
        [Header("Attack Timings")]
        public float castTime;
        public float buildUpTime;
        public float superArmor;
        public float activeTime;
        public float recoveryTime;
        public float cooldownTime;
        [Header("Rumble")]
        public GamePadRumbleWithTimer rumbleOnHit;
    }
}
