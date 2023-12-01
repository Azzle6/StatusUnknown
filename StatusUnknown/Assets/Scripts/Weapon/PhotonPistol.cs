using Input;

namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    using Combat.HitProcess;
    using Unity.Mathematics;
    using UnityEngine.VFX;

    
    public class PhotonPistol : RangedWeapon
    {
        [SerializeField] private PhotonPistolStat stat;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform mesh;
        [SerializeField] private Transform meshPos;
        [SerializeField] private VisualEffect chargingVFX;
        [SerializeField] private VisualEffect shootingVFX;
        private Vector3 initMeshPos;
        private float chargeTimer;
        private Coroutine charging;
        private Coroutine reloading;
        private Coroutine rumbleScale;
        private Projectile tempPhotonPistolBullet;
        private Transform tempPhotonPistolBulletTr;
        private float currentDamage;
        private bool waitForTriggerRelease;
        private bool isInCD;
        private float cdTimer;
        private bool isReloading;
        
        private void Start()
        {
            currentAmmo.Value = stat.magazineSize;
            initMeshPos = mesh.localPosition;
            chargingVFX.Stop();
        }

        private void OnDisable()
        {
            reloading = default;
            charging = default;
            chargingVFX.Stop();
        }

        public override void ActionPressed()
        {
            //disabling an object stop its coroutine so we need to check if it is already in cooldown and relaunch it
            if (isInCD)
            {
                StartCoroutine(Cooldown());
                return;
            }

            if ((isReloading) && (reloading == default))
            {
                Reload(weaponManager.playerAnimator);
                return;
            }
            
            if ((charging != default) || (isReloading) || (currentAmmo.Value <= 0))
                return;


            tempPhotonPistolBullet = ComponentPooler.Instance.GetPooledObject<Projectile>(stat.projectilePool.prefab.name);
            tempPhotonPistolBulletTr = tempPhotonPistolBullet.transform;
            
            charging = StartCoroutine(Charge());


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
        
        private IEnumerator Cooldown()
        {
            isInCD = true;
            yield return new WaitForSeconds(stat.cdTime);
            isInCD = false;
            
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
            tempPhotonPistolBullet.Launch(currentDamage, spawnPoint.forward, stat.projectileSpeed);
            tempPhotonPistolBulletTr.TryGetComponent(out HitContext tempHitContext);
            HitSphere tempHitSphere = tempHitContext.hitShape as HitSphere;
            tempHitSphere.radius = tempPhotonPistolBulletTr.localScale.y / 2;
            tempHitContext.HitTriggerEvent += tempPhotonPistolBullet.Hit;
            
            tempPhotonPistolBulletTr = default;
            tempPhotonPistolBullet = default;
            charging = default;
            currentAmmo.Value--;
        }

        public override float GetMagazineSize()
        {
            return stat.magazineSize;
        }

        public override void InitPool()
        {
            ComponentPooler.Instance.CreatePool(stat.projectilePool.prefab.GetComponent<Projectile>(),stat.projectilePool.baseCount);
        }

        public override void Reload(Animator playerAnimator)
        {
            if (reloading != default)
                return;
            
            weaponManager.rigLHand.weight = 0;
            weaponManager.rigRHand.weight = 0;
            mesh.transform.parent = weaponManager.rHandTr;
            StartCoroutine(ReloadingTimer());
            playerAnimator.SetTrigger("Reload");
        }

        public override void Switched(Animator playerAnimator, bool OnOff)
        {
            if (OnOff)
            {
                playerAnimator.SetLayerWeight(2,0);
                playerAnimator.SetLayerWeight(1,1);
                weaponManager.rigLHand.weight = 1;
                weaponManager.rigRHand.weight = 1;
            }
            else
            {
                ActionReleased();
            }
        }

        private IEnumerator ReloadingTimer()
        {
            isReloading = true;
            yield return new WaitForSeconds(stat.reloadTime);
            currentAmmo.Value = stat.magazineSize;
            isReloading = false;
            weaponManager.rigLHand.weight = 1;
            weaponManager.rigRHand.weight = 1;
            mesh.transform.parent = meshPos;
            mesh.transform.localRotation = quaternion.identity;
            mesh.transform.localPosition = initMeshPos; 
        }
        
        public override void Hit()
        {
            
        }
        
        
    }
}