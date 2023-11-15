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
            inputBufferActive = true;
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeAttack attack)
            {
                if (activeCoroutine == null)
                {
                    currentAttack = attack;
                    activeCoroutine = StartCoroutine(Active());
                }
       
            }
        }
        
        private IEnumerator Active()
        {
            weaponManager.GetCurrentMeleeWeapon().Active();
            yield return new WaitForSeconds(currentAttack.activeTime);
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            activeCoroutine = null;
            playerStateInterpretor.AddState("MeleeRecoveryPlayerState", PlayerStateType.ACTION, false);
            playerStateInterpretor.Behave(currentAttack,PlayerStateType.ACTION);
            currentAttack = null;
        }

        public override void OnStateExit()
        {
            
        }
    }
}
