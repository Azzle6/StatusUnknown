using UnityEngine;

namespace Core.Player
{
    public class PhotonPistol : Weapon
    {
        [SerializeField] private AnimationCurve damageCurve;
        [SerializeField] private float maxDamage;
        [SerializeField] private AnimationCurve projectileSize;
        [SerializeField] private float maxProjectileSize;
        [SerializeField] private float maxTimeCharge;
        [SerializeField] private float cdTime;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private Transform spawnPoint;
        private GameObject tempProjectile;
        
        public override void TriggerPressed()
        {
            
        }

        public override void TriggerReleased()
        {
         
        }

        public override void Reload()
        {
          
        }
    }
}

