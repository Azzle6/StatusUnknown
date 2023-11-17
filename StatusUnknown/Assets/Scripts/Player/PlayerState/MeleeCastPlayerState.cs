namespace Player
{
    using System.Collections;
    using UnityEngine;
    public class MeleeCastPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        private MeleeWeapon currentMeleeWeapon;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine castCoroutine;
        public override void OnStateEnter()
        {
          
        }

       
        public override void Behave<T>(T x)
        {
            if (x is MeleeAttack attack)
            {
                currentAttack = attack;
                if ((castCoroutine == null))
                {
                    castCoroutine = StartCoroutine(Cast());
                }
            }
        }

        
        
        private IEnumerator Cast()
        {
            //launch the cast animation
            //need to match animation length
            currentMeleeWeapon = playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon();
                if(currentMeleeWeapon == default)
                    yield break;
                    
            currentMeleeWeapon.Cast();
            yield return new WaitForSeconds(currentAttack.castTime);
          
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            playerStateInterpretor.AddState("MeleeBuildUpPlayerState", PlayerStateType.ACTION, true);
            MeleeAttack attackToBehave = currentAttack;
            currentAttack = null;
            playerStateInterpretor.Behave(attackToBehave,PlayerStateType.ACTION);
            castCoroutine = null;
            currentMeleeWeapon = null;
        }

            
        
        public override void OnStateExit()
        {
            
        }
    }
}
