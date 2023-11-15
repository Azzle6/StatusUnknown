namespace Player
{
    using DG.Tweening;
    using UnityEngine;

    public class WeaponManager : MonoBehaviour
    {
        public PlayerStateInterpretor playerStateInterpretor;
        [SerializeField] private Animator playerAnimator;
        public Weapon[] weapons;
        public int currentWeaponIndex;
        [SerializeField] private PlayerStat playerStat;
    
        private void Awake()
        {
            playerStat.weaponMelee[0] = weapons[0].meleeWeapon;
            playerStat.weaponMelee[1] = weapons[1].meleeWeapon;

            currentWeaponIndex = 0;
            weapons[1].gameObject.SetActive(false);
            RestWeapon();
        }

        public void EquipWeapon(int weaponNo, Weapon weapon)
        {
            weapons[weaponNo] = weapon;
            weapons[weaponNo].weaponManager = this;
            
            if (weapon.meleeWeapon)
                playerStat.weaponMelee[weaponNo] = true;
            else
                playerStat.weaponMelee[weaponNo] = false;
            
        }

    
        public void PressTriggerWeapon(int weaponNo)
        {
            if (currentWeaponIndex != weaponNo)
            {
                if (weapons[weaponNo].meleeWeapon)
                    weapons[currentWeaponIndex].adsRotTr.localRotation = Quaternion.Euler(weapons[weaponNo].adsRestAngle,0,0);
                
                weapons[currentWeaponIndex].gameObject.SetActive(false);
                weapons[weaponNo].gameObject.SetActive(true);
                currentWeaponIndex = weaponNo;

            }
            
            weapons[currentWeaponIndex].ActionPressed();
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
            return (MeleeWeapon) weapons[currentWeaponIndex];
        }
    }
}

