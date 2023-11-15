
using System.Collections;

namespace Player
{
    using UnityEngine;

    public class MeleeBuildUpPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private float superArmorDamageTaken;
        private Coroutine buildUpCoroutine;
        private float buildUpTimer;

    
        public override void OnStateEnter()
        {
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
            playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon().BuildUp();
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
                currentAttack = null;
                buildUpCoroutine = null;
                buildUpTimer = 0;
            }
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
