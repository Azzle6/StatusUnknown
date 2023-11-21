namespace Weapon
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class MeleeAttack
    {
        public float attackDamage;
        public float castTime;
        public float buildUpTime;
        public float superArmor;
        public float activeTime;
        public float recoveryTime;
        public float cooldownTime;
    }
}
