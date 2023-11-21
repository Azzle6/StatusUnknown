
using System.Collections;

namespace Player
{
    using UnityEngine;
    using Weapon;

    public class MeleeBuildUpPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private MeleeWeapon currentMeleeWeapon;
        private float superArmorDamageTaken;
        private Coroutine buildUpCoroutine;
        private float buildUpTimer;

    
        public override void OnStateEnter()
        {
            buildUpTimer = 0;
            superArmorDamageTaken = 0;
            //weaponManager.GetCurrentMeleeWeapon().BuildUp();
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeWeapon meleeWeapon)
            {
                currentMeleeWeapon = meleeWeapon;
                if (buildUpCoroutine == null)
                {
                    buildUpCoroutine = StartCoroutine(BuildUp());
                }
            }
        }


        private IEnumerator BuildUp()
        {
            currentMeleeWeapon.BuildUp();
            currentAttack = currentMeleeWeapon.GetAttack();
            while (buildUpTimer <= currentAttack.buildUpTime)
            {
                if (superArmorDamageTaken > currentAttack.superArmor)
                {
                    playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                    yield break;
                }
                buildUpTimer += Time.deltaTime;
                yield return null;
            }
            if (playerStateInterpretor.CheckState(PlayerStateType.ACTION,"MeleeBuildUpPlayerState"))
            {
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                superArmorDamageTaken = 0;
                playerStateInterpretor.AddState("MeleeActivePlayerState", PlayerStateType.ACTION, true);
                playerStateInterpretor.Behave(currentMeleeWeapon,PlayerStateType.ACTION);
               
            }
            currentAttack = default;
            buildUpCoroutine = default;
            buildUpTimer = 0;
            currentMeleeWeapon = default;
        }
        
        public void SuperArmorTakeDamage(float damage)
        {
            superArmorDamageTaken += damage;
        }

        public override void OnStateExit()
        {
  
        }
    }
}
