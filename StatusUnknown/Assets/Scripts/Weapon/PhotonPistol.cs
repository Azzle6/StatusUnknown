using Input;
using UnityEngine.VFX.Utility;

namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    using Combat.HitProcess;
    using Module.Behaviours;
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
        
        private void Awake()
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
            chargingVFX.SetFloat("Size", 0);
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
              chargingVFX.SetFloat("Size", stat.projectileSize.Evaluate(chargeTimer / stat.maxTimeCharge));
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
            chargingVFX.SetFloat("Size", 0);
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
            tempPhotonPistolBullet.Launch(currentDamage, spawnPoint.rotation, stat.projectileSpeed);
            Transform pistolTransform = this.tempPhotonPistolBullet.transform;
            bool isFullyCharged = this.chargeTimer >= this.stat.maxTimeCharge;
            this.tempPhotonPistolBullet.onHit += () => OnProjectileHit(isFullyCharged, pistolTransform);
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
            if(isFullCharged) ModuleBehaviourHandler.Instance.CastModule(this.inventory, this.weaponDefinition,E_WeaponOutput.ON_HIT_FULL_CHARGED, projectile);
        }
        
        public override void Hit()
        {
            
        }
        
        
    }
}