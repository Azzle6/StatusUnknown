using UnityEngine.Animations.Rigging;

namespace Player
{
    using DG.Tweening;
    using UnityEngine;

    public class WeaponManager : MonoBehaviour
    {
        public PlayerStateInterpretor playerStateInterpretor;
        public Animator playerAnimator;
        public Weapon[] weapons;
        public int currentWeaponIndex;
        [SerializeField] private PlayerStat playerStat;
        public Transform lHandTr;
        public Transform rHandTr;
        public Rig rigLHand;
        public Rig rigRHand;
    
        private void Awake()
        {
            InitWeaponManager();
        }

        private void InitWeaponManager()
        {
            playerStat.weaponMelee[0] = weapons[0].meleeWeapon;
            playerStat.weaponMelee[1] = weapons[1].meleeWeapon;

            currentWeaponIndex = 1;
            SwitchWeapon(0);
            RestWeapon();
        }

        public void EquipWeapon(int weaponNo, Weapon weapon)
        {
            weapons[weaponNo] = weapon;
            weapons[weaponNo].weaponManager = this;
            
            playerStat.weaponMelee[weaponNo] = weapon.meleeWeapon;
        }

    
        public void PressTriggerWeapon(int weaponNo)
        {
            if (currentWeaponIndex != weaponNo)
                SwitchWeapon(weaponNo);
            
            weapons[currentWeaponIndex].ActionPressed();
        }

        private void SwitchWeapon(int weaponNo)
        {
            if (weapons[weaponNo].meleeWeapon)
            {
                //changing arm layer
                playerStateInterpretor.animator.SetLayerWeight(2,1);
                playerStateInterpretor.animator.SetLayerWeight(1,0);

            }
            else
            {
                playerStateInterpretor.animator.SetLayerWeight(2,0);
                playerStateInterpretor.animator.SetLayerWeight(1,1);
            }
                
            weapons[currentWeaponIndex].Switched(playerAnimator, false);
            weapons[weaponNo].gameObject.SetActive(true);
            weapons[weaponNo].Switched(playerAnimator, true);
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = weaponNo;
        }
    
    
        public void ReleaseTriggerWeapon()
        {
            weapons[currentWeaponIndex].ActionReleased();
        }

        public void AimWithCurrentWeapon()
        {
            if (weapons[currentWeaponIndex].meleeWeapon)
                return;
            
            weapons[currentWeaponIndex].adsRotTr.DOLocalRotate(new Vector3(weapons[currentWeaponIndex].adsAimAngle,0,0), 0.1f);
        }
    
        public void RestWeapon()
        {
            if (weapons[currentWeaponIndex].meleeWeapon)
                return;
            
            weapons[currentWeaponIndex].adsRotTr.DOLocalRotate(new Vector3(weapons[currentWeaponIndex].adsRestAngle,0,0), 0.1f);
        }

        public void ReloadLastEquipedWeapon()
        {
            weapons[currentWeaponIndex].Reload(playerAnimator);
        }
        
        public Weapon GetCurrentWeapon()
        {
            return weapons[currentWeaponIndex];
        }
        
        public MeleeWeapon GetCurrentMeleeWeapon()
        {
            if (weapons[currentWeaponIndex].meleeWeapon == false)
                return null;
            
            return (MeleeWeapon) weapons[currentWeaponIndex];
        }
    }
}

