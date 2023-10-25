namespace Core.Player
{
    using System.Collections;
    using UnityEngine;
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
        private bool waitForTriggerRelease;
        private bool isInCD;
        
        public override void TriggerPressed()
        {
            if ((charging != default) || (isInCD))
                return;
            tempProjectile = Pooler.Instance.GetPooledObject("PlayerPhotonProjectile");
            charging = StartCoroutine(Charge());
        }
        
        private IEnumerator Charge()
        {
            float timer = 0;
            tempProjectile.transform.parent = spawnPoint;
            while (timer < maxTimeCharge)
            {
              tempProjectile.transform.localPosition = Vector3.zero;
              timer += Time.deltaTime;
              tempProjectile.transform.localScale = Vector3.one * (projectileSize.Evaluate(timer / maxTimeCharge) * maxProjectileSize);
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
            yield return new WaitForSeconds(cdTime);
            isInCD = false;
            
        }

        public override void TriggerReleased()
        {
            Debug.Log("Release");
            if (charging == default)
                return;
            waitForTriggerRelease = false;
            StartCoroutine(Cooldown());
            StopCoroutine(charging);
            tempProjectile.transform.parent = null;
            tempProjectile.TryGetComponent(out Rigidbody tempRb);
            tempRb.velocity = spawnPoint.forward * projectileSpeed;
            tempProjectile = default;
            charging = default;
        }

        public override void Reload()
        {
          
        }
    }
}