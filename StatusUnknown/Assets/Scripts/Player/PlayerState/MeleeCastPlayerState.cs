namespace Player
{
    using System.Collections;
    using UnityEngine;
    public class MeleeCastPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine castCoroutine;
        public override void OnStateEnter()
        {
          
        }

       
        public override void Behave<T>(T x) 
        {
            
            if (x is MeleeAttack attack)
            {
                if (castCoroutine == null)
                {
                    currentAttack = attack;
                    castCoroutine = StartCoroutine(Cast());
                }
            }
        }

        
        
        private IEnumerator Cast()
        {
            //launch the cast animation
            //need to match animation length
            playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon().Cast();
            yield return new WaitForSeconds(currentAttack.castTime);
            if (playerStateInterpretor.CheckState(PlayerStateType.ACTION,"MeleeCastPlayerState"))
            {
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                playerStateInterpretor.AddState("MeleeBuildUpPlayerState", PlayerStateType.ACTION, true);
                playerStateInterpretor.Behave(currentAttack,PlayerStateType.ACTION);
            }
          
            castCoroutine = null;
            currentAttack = null;
        }
        
        public override void OnStateExit()
        {
            
        }
    }
}
