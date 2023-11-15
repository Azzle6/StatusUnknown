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
                    Debug.Log("cast behave received attack");
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
            Debug.Log("cast finished");
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            Debug.Log("exiting cast");
            playerStateInterpretor.AddState("MeleeBuildUpPlayerState", PlayerStateType.ACTION, false);
            playerStateInterpretor.Behave(currentAttack,PlayerStateType.ACTION);
            castCoroutine = null;
            currentAttack = null;
        }
        
        public override void OnStateExit()
        {
            
        }
    }
}
