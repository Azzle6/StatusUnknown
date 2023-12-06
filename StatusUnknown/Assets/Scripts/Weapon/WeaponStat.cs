using Core.Pooler;
using UnityEngine.Serialization;

namespace Weapon
{
    using UnityEngine;
    public class WeaponStat : ScriptableObject
    { 
        public int weaponID;
        
        [Header("Magazine")]
        public int magazineSize;
        public float reloadTime;
        [Header("Projectile Pool")]
        public CoPoolProjectile projectilePool;
        public float fireRate;


    }
}