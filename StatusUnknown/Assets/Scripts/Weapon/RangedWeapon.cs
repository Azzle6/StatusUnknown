using System;
using Core.Pooler;

namespace Weapon
{
    using Core.VariablesSO.VariableTypes;
    using DG.Tweening;
    using UnityEngine;
    using Unity.Mathematics;
    using System.Collections;

    using Weapons;

    
    public abstract class RangedWeapon : Weapon
    {
        [Header("Ads angle")]
        public Transform adsRotTr;
        public float adsAimAngle;
        public float adsRestAngle;
        public Coroutine reloading;
        public Transform spawnPoint;
        [SerializeField] private Vector3 initMeshPos;

        public WeaponStat weaponStat;
        public FloatVariableSO currentAmmo;
        public Transform mesh;
        public Transform meshPos;
        
        [HideInInspector] public bool isInCD;

        private void Start()
        {
            currentAmmo.Value = weaponStat.magazineSize;
            initMeshPos = mesh.localPosition;
        }
        
        private void OnDisable()
        {
            reloading = default;
        }

        public override void AimWithCurrentWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsAimAngle,0,0), 0.1f);
        }
    
        public override void RestWeapon()
        {
            adsRotTr.DOLocalRotate(new Vector3(adsRestAngle,0,0), 0.1f);
        }

        public override bool ActionPressed()
        {
            //disabling an object stop its coroutine so we need to check if it is already in cooldown and relaunch it
            if (isInCD)
            {
                StartCoroutine(Cooldown());
                return false;
            }

            if ((reloading != default) || (currentAmmo.Value <= 0))
            {
                Reload(weaponManager.playerAnimator);
                return false;
            }
            return true;
        }
        
        public override void Reload(Animator playerAnimator)
        {
            if (reloading != default)
                return;
            
            CastModule(E_WeaponOutput.ON_RELOAD, spawnPoint);
            weaponManager.SwitchHandRigs(false);
            mesh.transform.parent = weaponManager.rHandTr;
            reloading = StartCoroutine(ReloadingTimer());
            playerAnimator.SetTrigger("Reload");
        }
        
        private IEnumerator ReloadingTimer()
        {
            //isReloading = true;
            yield return new WaitForSeconds(weaponStat.reloadTime);
            currentAmmo.Value = weaponStat.magazineSize;
            //isReloading = false;
            weaponManager.SwitchHandRigs(true);
            mesh.transform.parent = meshPos;
            mesh.transform.localRotation = quaternion.identity;
            mesh.transform.localPosition = initMeshPos; 
            reloading = default;
        }


        public float GetMagazineSize()
        {
            return weaponStat.magazineSize;
        }
        
        public IEnumerator Cooldown()
        {
            isInCD = true;
            yield return new WaitForSeconds(weaponStat.fireRate);
            isInCD = false;
        }

        public void InitPool()
        {
            Debug.Log("pool of bullet created");
            ComponentPooler.Instance.CreatePool(weaponStat.projectilePool.prefab.GetComponent<Projectile>(),weaponStat.projectilePool.baseCount);
        }
        
        public override void Switched(Animator playerAnimator, bool OnOff)
        {
            if (OnOff)
            {
                playerAnimator.SetLayerWeight(2,0);
                playerAnimator.SetLayerWeight(1,1);
            }
            else
            {
                ActionReleased();
            }
        }

    }
}


