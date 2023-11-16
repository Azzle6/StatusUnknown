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
            recoveryCoroutine = StartCoroutine(Recovery());
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeAttack attack)
            {
                currentAttack = attack;
            }
        }
        
        private IEnumerator Recovery()
        {
            while (currentAttack == null)
                yield return null;
            
            playerStateInterpretor.weaponManager.GetCurrentMeleeWeapon().Recovery();
            yield return new WaitForSeconds(currentAttack.recoveryTime);
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            recoveryCoroutine = null;
        }

        public override void OnStateExit()
        {
            playerStateInterpretor.ExecuteBufferInput();
        }
    }
}
