namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    using Module.Behaviours;
    using UnityEngine.VFX;
    using Weapons;
    using Input;



    public class PhotonPistol : RangedWeapon
    {
        private PhotonPistolStat stat;
        [SerializeField] private VisualEffect shootingVFX;
        [SerializeField] private VisualEffectAsset tinyProjectileVFX;
        [SerializeField] private VisualEffectAsset chargedProjectileVFX;
        private VisualEffect tempProjectileVFX;
        private float chargeVFXSize;
        private float chargeTimer;
        private Coroutine charging;
        private Coroutine rumbleScale;
        private Projectile tempPhotonPistolBullet;
        private Transform tempPhotonPistolBulletTr;
        private float currentDamage;
        private bool waitForTriggerRelease;
        private float cdTimer;
        private bool fullyCharged;
        
        private void Awake()
        {
            stat = weaponStat as PhotonPistolStat;
        }

        private void OnDisable()
        {
            reloading = default;
            charging = default;
        }

        public override bool ActionPressed()
        {
            if (!base.ActionPressed())
            {
                return false;
            }

            if (charging != default)
                return false;

            if (reloading != default)
                return false;
            
            fullyCharged = false;
            tempPhotonPistolBullet = ComponentPooler.Instance.GetPooledObject<Projectile>(stat.projectilePool.prefab.name);
            tempPhotonPistolBulletTr = tempPhotonPistolBullet.transform;
            tempPhotonPistolBulletTr.forward = spawnPoint.forward;
            shootingVFX = tempPhotonPistolBullet.GetProjectileVFX();
            shootingVFX.visualEffectAsset = tinyProjectileVFX;
            shootingVFX.SetFloat("Size", 0);
            charging = StartCoroutine(Charge());
            return true;
        }
        
        private IEnumerator Charge()
        {
            chargeTimer = 0;
            tempPhotonPistolBulletTr.parent = spawnPoint;
            shootingVFX.enabled = true;
            shootingVFX.Play();
            rumbleScale ??= StartCoroutine(GamePadRumbleManager.ExecuteRumbleWithTime(stat.rumbleScaling, false));
            while (chargeTimer < stat.maxTimeCharge)
            {
              tempPhotonPistolBulletTr.localPosition = Vector3.zero;
              chargeTimer += Time.deltaTime;
              tempPhotonPistolBulletTr.localScale = Vector3.one * (stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxProjectileSize);
              chargeVFXSize = stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxProjectileSize;

              if (chargeVFXSize >= stat.projectileTinytoChargedTreshold &&
                  shootingVFX.visualEffectAsset != chargedProjectileVFX)
              {
                  shootingVFX.visualEffectAsset = chargedProjectileVFX;
              }
              
                  
              shootingVFX.SetFloat("Size", stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge));
              currentDamage = stat.damageCurve.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxDamage;
              yield return null;
            }
            fullyCharged = true;
            StartCoroutine(WaitForTriggerRelease());
        }
        
        private IEnumerator WaitForTriggerRelease()
        {
            waitForTriggerRelease = true;
            while (waitForTriggerRelease)
            {
                tempPhotonPistolBulletTr.localPosition = Vector3.zero;
                yield return null;
            }
        }

        public override void ActionReleased()
        {
            if (tempPhotonPistolBullet == default)
                return;
                
            waitForTriggerRelease = false;
            StartCoroutine(Cooldown());
            if (charging != default)
                StopCoroutine(charging);

            if (rumbleScale != default)
            {
                StopCoroutine(rumbleScale);
                GamePadRumbleManager.StopRumble(); 
                rumbleScale = default;
                StartCoroutine(
                    GamePadRumbleManager.ExecuteRumbleWithTime(stat.rumbleOnShoot, true, chargeTimer / stat.maxTimeCharge));
            }
            
            //change projectile size
            tempProjectileVFX = tempPhotonPistolBullet.GetProjectileVFX();
            tempProjectileVFX.SetFloat("Size", chargeVFXSize);
            shootingVFX.Play();
            tempPhotonPistolBulletTr.transform.parent = null;
            tempPhotonPistolBullet.Launch(currentDamage, spawnPoint.rotation, stat.projectileSpeed,stat.fullyChargedDamage,stat.fullyChargedRadius,fullyCharged);
            Transform pistolTransform = tempPhotonPistolBullet.transform;
            bool isFullyCharged = chargeTimer >= stat.maxTimeCharge;
            tempPhotonPistolBullet.onHit += () => OnProjectileHit(isFullyCharged, pistolTransform);
            tempPhotonPistolBullet.StartCheckingCollision();
            tempPhotonPistolBullet.hitShape.radius = tempPhotonPistolBulletTr.localScale.y / 2;
            
            ModuleBehaviourHandler.Instance.CastModule(this.inventory, this.weaponDefinition,E_WeaponOutput.ON_SPAWN, spawnPoint);
            
            tempPhotonPistolBulletTr = default;
            tempPhotonPistolBullet = default;
            charging = default;
            currentAmmo.Value--;
        }

        private void OnProjectileHit(bool isFullCharged, Transform projectile)
        {
            ModuleBehaviourHandler.Instance.CastModule(this.inventory, this.weaponDefinition, E_WeaponOutput.ON_HIT, projectile);
            if (isFullCharged)
            {
                ModuleBehaviourHandler.Instance.CastModule(this.inventory, this.weaponDefinition,E_WeaponOutput.ON_HIT_FULL_CHARGED, projectile);
            }
        }
        
        public override void Hit()
        {
            
        }
        
        
    }
}