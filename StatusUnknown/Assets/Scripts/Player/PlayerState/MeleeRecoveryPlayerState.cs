using System.Collections;

namespace Player
{
    using UnityEngine;

    public class MeleeRecoveryPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine recoveryCoroutine;

        public override void OnStateEnter()
        {
            inputBufferActive = true;
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeAttack attack)
            {
                currentAttack = attack;
                recoveryCoroutine = StartCoroutine(Recovery());
            }
        }
        
        private IEnumerator Recovery()
        {
            playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon().Recovery();
            yield return new WaitForSeconds(currentAttack.recoveryTime);
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
        }

        public override void OnStateExit()
        {
            playerStateInterpretor.ExecuteBufferInput();
        }
    }
}
