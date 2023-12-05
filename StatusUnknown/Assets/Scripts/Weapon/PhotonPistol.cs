using Input;

namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    using Combat.HitProcess;
    using Unity.Mathematics;
    using UnityEngine.VFX;
    using Weapons;


    public class PhotonPistol : RangedWeapon
    {
        private PhotonPistolStat stat;
        [SerializeField] private VisualEffect chargingVFX;
        [SerializeField] private VisualEffect shootingVFX;
        private float chargeTimer;
        private Coroutine charging;
        private Coroutine rumbleScale;
        private Projectile tempPhotonPistolBullet;
        private Transform tempPhotonPistolBulletTr;
        private float currentDamage;
        private bool waitForTriggerRelease;
        private float cdTimer;
        
        private void Start()
        {
            chargingVFX.Stop();
            stat = weaponStat as PhotonPistolStat;
        }

        private void OnDisable()
        {
            reloading = default;
            charging = default;
            chargingVFX.Stop();
        }

        public override bool ActionPressed()
        {
            if (!base.ActionPressed())
            {
                return false;
            }

            if (charging != default)
                return false;

            tempPhotonPistolBullet = ComponentPooler.Instance.GetPooledObject<Projectile>(stat.projectilePool.prefab.name);
            tempPhotonPistolBulletTr = tempPhotonPistolBullet.transform;
            
            charging = StartCoroutine(Charge());
            return true;
        }
        
        private IEnumerator Charge()
        {
            chargeTimer = 0;
            tempPhotonPistolBulletTr.parent = spawnPoint;
            chargingVFX.Play();
            rumbleScale ??= StartCoroutine(GamePadRumbleManager.ExecuteRumbleWithTime(stat.rumbleScaling, false));
            while (chargeTimer < stat.maxTimeCharge)
            {
              tempPhotonPistolBulletTr.localPosition = Vector3.zero;
              chargeTimer += Time.deltaTime;
              tempPhotonPistolBulletTr.localScale = Vector3.one * (stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge) * stat.maxProjectileSize);
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
                tempPhotonPistolBulletTr.localPosition = Vector3.zero;
                yield return null;
            }
        }

        public override void ActionReleased()
        {
            if (tempPhotonPistolBullet == default)
                return;
            
            chargingVFX.Stop();
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
            
            shootingVFX.Play();
            tempPhotonPistolBulletTr.transform.parent = null;
            tempPhotonPistolBullet.Launch(currentDamage, spawnPoint.forward, stat.projectileSpeed, this.inventory.GetWeaponTriggerData(this.weaponDefinition, E_WeaponOutput.ON_HIT).compiledModules.FirstModule);
            tempPhotonPistolBulletTr.TryGetComponent(out HitContext tempHitContext);
            HitSphere tempHitSphere = tempHitContext.hitShape as HitSphere;
            tempHitSphere.radius = tempPhotonPistolBulletTr.localScale.y / 2;
            tempHitContext.HitTriggerEvent += tempPhotonPistolBullet.Hit;
            
            this.CastModule(E_WeaponOutput.ON_SPAWN, this.spawnPoint);
            
            tempPhotonPistolBulletTr = default;
            tempPhotonPistolBullet = default;
            charging = default;
            currentAmmo.Value--;
        }
        
        
        public override void Hit()
        {
            
        }
        
        
    }
}