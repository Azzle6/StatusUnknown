using System.Collections;
using UnityEngine.Rendering.HighDefinition;

namespace Player
{
    using UnityEngine;

    public class MeleeActivePlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine activeCoroutine;

        public override void OnStateEnter()
        {

            Debug.Log("Active melee weapon");
            inputBufferActive = true;
        }
        
        public override void Behave<T>(T x) 
        {
            Debug.Log("active behave");
            if (x is MeleeAttack attack)
            {
                Debug.Log("active behave received attack");
                if (activeCoroutine == null)
                {
                    Debug.Log("active behave received attack and active coroutine was not null");
                    currentAttack = attack;
                    activeCoroutine = StartCoroutine(Active());
                }
       
            }
        }
        
        private IEnumerator Active()
        {
            weaponManager.GetCurrentMeleeWeapon().Active();
            Debug.Log("starting active coroutine");
            yield return new WaitForSeconds(currentAttack.activeTime);
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            activeCoroutine = null;
            Debug.Log("exiting Active");
            playerStateInterpretor.AddState("MeleeRecoveryPlayerState", PlayerStateType.ACTION, false);
            playerStateInterpretor.Behave(currentAttack,PlayerStateType.ACTION);
            currentAttack = null;
        }

        public override void OnStateExit()
        {
            
        }
    }
}
