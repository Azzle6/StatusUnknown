
using System.Collections;

namespace Player
{
    using UnityEngine;

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
            if (x is MeleeAttack attack)
            {
                currentAttack = attack;
                if (buildUpCoroutine == null)
                    buildUpCoroutine = StartCoroutine(BuildUp());
            }
        }


        private IEnumerator BuildUp()
        {
            currentMeleeWeapon = playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon();
            if (currentMeleeWeapon == default)
                yield break;
            
            currentMeleeWeapon.BuildUp();
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
                playerStateInterpretor.Behave(currentAttack,PlayerStateType.ACTION);
               
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
