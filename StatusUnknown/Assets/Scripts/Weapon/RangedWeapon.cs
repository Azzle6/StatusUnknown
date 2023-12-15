using System;
using Core.EventsSO.GameEventsTypes;
using Core.Pooler;

namespace Weapon
{
    using Core.VariablesSO.VariableTypes;
    using DG.Tweening;
    using UnityEngine;
    using Unity.Mathematics;
    using System.Collections;
    using Module.Behaviours;
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
        [HideInInspector] public FloatGameEvent reloadEvent;

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

        private void OnEnable()
        {
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

            if ((currentAmmo.Value <= 0) && (reloading == default))
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
            
            ModuleBehaviourHandler.Instance.CastModule(this.inventory, this.weaponDefinition,E_WeaponOutput.ON_RELOAD, spawnPoint, null);
            weaponManager.SwitchHandRigs(false);
            mesh.transform.parent = weaponManager.rHandTr;
            mesh.transform.position = weaponManager.rHandTr.position;
            reloading = StartCoroutine(ReloadingTimer());
            reloadEvent.RaiseEvent(weaponStat.reloadTime);
            playerAnimator.SetTrigger("Reload");
        }
        
        private IEnumerator ReloadingTimer()
        {
            //isReloading = true;
            yield return new WaitForSeconds(weaponStat.reloadTime);
            currentAmmo.Value = weaponStat.magazineSize;
            //isReloading = false;
            weaponManager.SwitchHandRigs(true);
            ResetReloadTransform();
   
        }

        public void ResetReloadTransform()
        {
            if (reloading != default)
            {
                StopCoroutine(reloading);
            }
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
                ActionReleased();
                playerAnimator.SetLayerWeight(2,0);
                playerAnimator.SetLayerWeight(1,1);
                
            }
            else
            {
                ResetReloadTransform();
            }
        }

    }
}


