using System.Collections;
using UnityEngine;

namespace Core.Player
{
    using Core.Pooler;
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
        private Coroutine charging;
        private GameObject tempProjectile;
        
        public override void TriggerPressed()
        {
            if (charging != default)
                return;
            tempProjectile = Pooler.Instance.GetPooledObject("PlayerPhotonProjectile");
            tempProjectile.transform.position = spawnPoint.position;
            charging = StartCoroutine(Charge());
        }
        
        private IEnumerator Charge()
        {
            float timer = 0;
            while (timer < maxTimeCharge)
            {
                timer += Time.deltaTime;
                tempProjectile.transform.localScale = tempProjectile.transform.localScale * (projectileSize.Evaluate(timer / maxTimeCharge) * maxProjectileSize);
                yield return null;
            }
        }

        public override void TriggerReleased()
        {
         
        }

        public override void Reload()
        {
          
        }
    }
}