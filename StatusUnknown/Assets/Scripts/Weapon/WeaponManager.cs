using Core.VariablesSO.VariableTypes;

namespace Weapon
{
    using Player;
    using UnityEngine;
    using UnityEngine.Animations.Rigging;


    public class WeaponManager : MonoBehaviour
    {
        public PlayerStateInterpretor playerStateInterpretor;
        public Animator playerAnimator;
        public Weapon[] weapons;
        public WeaponVariableSO[] weaponsSO;
        public int currentWeaponIndex;
        [SerializeField] private PlayerStat playerStat;
        public EnemyStatusHandler enemyStatusHandler;
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

            currentWeaponIndex = 1;
            playerStat.currentWeaponIsMelee = CheckIfMeleeWeapon(0);
            SwitchWeapon(0);

            if (weapons[0].TryGetComponent(out RangedWeapon rangedWeapon))
                rangedWeapon.RestWeapon();

            for (int x = 0; x < weapons.Length; x++)
                weaponsSO[x].Value = weapons[x];

        }

        public void EquipWeapon(int weaponNo, Weapon weapon)
        {
            weapons[weaponNo] = weapon;
            weapons[weaponNo].weaponManager = this;
            weaponsSO[weaponNo].Value = weapon;
        }
        
        public void PressTriggerWeapon(int weaponNo)
        {
            weapons[currentWeaponIndex].ActionPressed();
        }

        public void SwitchWeapon(int weaponNo)
        {
            if (weaponNo == currentWeaponIndex)
                return;
            
            if (CheckIfMeleeWeapon(weaponNo))
            {
                //changing arm layer
                playerStateInterpretor.animator.SetLayerWeight(2,1);
                playerStateInterpretor.animator.SetLayerWeight(1,0);
                playerStat.currentWeaponIsMelee = true;
            }
            else
            {
                playerStateInterpretor.animator.SetLayerWeight(2,0);
                playerStateInterpretor.animator.SetLayerWeight(1,1);
                playerStat.currentWeaponIsMelee = false;
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
        
        public void ReloadWeapon()
        {
            if (weapons[currentWeaponIndex].TryGetComponent(out RangedWeapon rangedWeapon))
                rangedWeapon.Reload(playerAnimator);
        }

        public void RestWeapon()
        {
            if (weapons[currentWeaponIndex].TryGetComponent(out RangedWeapon rangedWeapon))
                rangedWeapon.RestWeapon();
        }
        
        public void AimWithCurrentWeapon()
        {
            if (weapons[currentWeaponIndex].TryGetComponent(out RangedWeapon rangedWeapon))
                rangedWeapon.AimWithCurrentWeapon();
        }
        
        public Weapon GetCurrentWeapon()
        {
            return weapons[currentWeaponIndex];
        }
        
        public MeleeWeapon ReturnIfMeleeWeapon(int weaponNo)
        {
            if (weapons[weaponNo].TryGetComponent(out MeleeWeapon meleeWeapon))
                return meleeWeapon;
            else
                return null;
        }
        
        public RangedWeapon ReturnIfRangedWeapon(int weaponNo)
        {
            if (weapons[weaponNo].TryGetComponent(out RangedWeapon rangedWeapon))
                return rangedWeapon;
            else
                return null;
        }
        
        public bool CheckIfMeleeWeapon(int weaponNo)
        {
            if (weapons[weaponNo].TryGetComponent(out MeleeWeapon meleeWeapon))
                return true;
            else
                return false;
        }
        
        public bool CheckIfRangedWeapon(int weaponNo)
        {
            if (weapons[weaponNo].TryGetComponent(out RangedWeapon rangedWeapon))
                return true;
            else
                return false;
        }
        
        
    }
}

