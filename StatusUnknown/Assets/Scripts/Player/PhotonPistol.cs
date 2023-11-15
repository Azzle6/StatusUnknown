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
        private float currentDamage;
        private bool waitForTriggerRelease;
        private bool isInCD;
        private bool isReloading;
        private float currentAmmo;
        
        private void Awake()
        {
            currentAmmo = stat.magazineSize;
        }
        
        public override void ActionPressed()
        {
            if ((charging != default) || (isInCD) || (isReloading) || (currentAmmo <= 0))
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
              currentDamage = stat.damageCurve.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxDamage;
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

        public override void ActionReleased()
        {
            if (charging == default)
                return;
            waitForTriggerRelease = false;
            StartCoroutine(Cooldown());
            StopCoroutine(charging);
            tempProjectile.transform.parent = null;
            tempProjectile.TryGetComponent(out Rigidbody tempRb);
            tempRb.velocity = spawnPoint.forward * stat.projectileSpeed;
            tempProjectile.TryGetComponent(out PhotonPistolBullet tempPPbullet);
            tempPPbullet.damage = currentDamage;
            tempProjectile = default;
            charging = default;
            currentAmmo--;
        }

        public override void Reload(Animator playerAnimator)
        {
            if (isReloading)
                return;
            StartCoroutine(ReloadingTimer());
            playerAnimator.SetTrigger("Reload");
        }

        private IEnumerator ReloadingTimer()
        {
            isReloading = true;
            yield return new WaitForSeconds(stat.reloadTime);
            currentAmmo = stat.magazineSize;
            isReloading = false;
        }
        
        public override void Hit()
        {
            
        }
    }
}