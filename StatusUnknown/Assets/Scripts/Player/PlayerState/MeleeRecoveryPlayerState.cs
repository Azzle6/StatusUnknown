using System.Collections;

namespace Player
{
    using UnityEngine;

    public class MeleeRecoveryPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [HideInInspector] public MeleeWeapon currentMeleeWeapon;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine recoveryCoroutine;

        public override void OnStateEnter()
        {
            inputBufferActive = true;
            recoveryCoroutine = StartCoroutine(Recovery());
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeWeapon meleeWeapon)
            {
                currentMeleeWeapon = meleeWeapon;
            }
        }
        
        private IEnumerator Recovery()
        {
            while (currentMeleeWeapon == null)
                yield return null;
            
            currentMeleeWeapon.Recovery();
            currentAttack = currentMeleeWeapon.GetAttack();
            yield return new WaitForSeconds(currentAttack.recoveryTime);
            playerStateInterpretor.ExecuteBufferInput();
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            currentAttack = null;
            currentMeleeWeapon = null;
            recoveryCoroutine = null;
        }

        public override void OnStateExit()
        {
        }
    }
}
