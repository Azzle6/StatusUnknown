

using Core.VariablesSO.VariableTypes;

namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Core.Pooler;
    using System;
    using Unity.Mathematics;
    
    public class PhotonPistol : RangedWeapon
    {
        [SerializeField] private PhotonPistolStat stat;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform mesh;
        [SerializeField] private Transform meshPos;
        private Vector3 initMeshPos;
        private float chargeTimer;
        private Coroutine charging;
        private Coroutine reloading;
        private GameObject tempProjectile;
        private float currentDamage;
        private bool waitForTriggerRelease;
        private bool isInCD;
        private float cdTimer;
        private bool isReloading;
        
        private void Awake()
        {
            currentAmmo.Value = stat.magazineSize;
            initMeshPos = mesh.localPosition;
        }

        private void OnDisable()
        {
            reloading = default;
            charging = default;
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
                Debug.Log("need to resume reload animation");
                Reload(weaponManager.playerAnimator);
                return;
            }
            
            if ((charging != default) || (isReloading) || (currentAmmo.Value <= 0))
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
            if (tempProjectile == default)
                return;
            
            waitForTriggerRelease = false;
            StartCoroutine(Cooldown());
            if (charging != default)
                StopCoroutine(charging);
            
            tempProjectile.transform.parent = null;
            tempProjectile.TryGetComponent(out Rigidbody tempRb);
            tempRb.velocity = spawnPoint.forward * stat.projectileSpeed;
            tempProjectile.TryGetComponent(out PhotonPistolBullet tempPPbullet);
            tempPPbullet.damage = currentDamage;
            tempProjectile.TryGetComponent(out HitContext tempHitContext);
            HitSphere tempHitSphere = tempHitContext.hitShape as HitSphere;
            tempHitSphere.radius = tempProjectile.transform.localScale.y / 2;
            tempHitContext.HitTriggerEvent += tempPPbullet.Hit;
            
            tempProjectile = default;
            charging = default;
            currentAmmo.Value--;
        }

        public override float GetMagazineSize()
        {
            return stat.magazineSize;
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