namespace Player
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    
    public class PhotonPistol : Weapon
    {
        [SerializeField] private PhotonPistolStat stat;
        [SerializeField] private Transform spawnPoint;
        private float chargeTimer;
        private Coroutine charging;
        private GameObject tempProjectile;
        private bool waitForTriggerRelease;
        private bool isInCD;
        
        public override void TriggerPressed()
        {
            if ((charging != default) || (isInCD))
                return;
            tempProjectile = Pooler.Instance.GetPooledObject(stat.projectilePrefab.name);
            charging = StartCoroutine(Charge());
        }
        
        private IEnumerator Charge()
        {
            chargeTimer = 0;
            tempProjectile.transform.parent = spawnPoint;
            while (chargeTimer < stat.maxTimeCharge)
            {
              tempProjectile.transform.localPosition = Vector3.zero;
              chargeTimer += Time.deltaTime;
              tempProjectile.transform.localScale = Vector3.one * (stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxProjectileSize);
              yield return null;
            }

            StartCoroutine(WaitForTriggerRelease());
        }
        
        private IEnumerator WaitForTriggerRelease()
        {
            waitForTriggerRelease = true;
            while (waitForTriggerRelease)
            {
                tempProjectile.transform.localPosition = Vector3.zero;
                yield return null;
            }
        }
        
        private IEnumerator Cooldown()
        {
            isInCD = true;
            yield return new WaitForSeconds(stat.cdTime);
            isInCD = false;
            
        }

        public override void TriggerReleased()
        {
            if (charging == default)
                return;
            waitForTriggerRelease = false;
            StartCoroutine(Cooldown());
            StopCoroutine(charging);
            tempProjectile.transform.parent = null;
            tempProjectile.TryGetComponent(out Rigidbody tempRb);
            tempRb.velocity = spawnPoint.forward * stat.projectileSpeed;
            tempProjectile = default;
            charging = default;
        }

        public override void Reload()
        {
          
        }

        public override void Hit()
        {
            
        }
    }
}