namespace Player
{
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
        public GameObject projectilePrefab;
        [Header("Charge")]
        public float maxTimeCharge;
        public float cdTime;
        [Header("Magazine")]
        public int magazineSize;
        public float reloadTime;
    }
}

