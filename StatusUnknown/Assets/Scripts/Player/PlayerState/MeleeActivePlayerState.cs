

namespace Player
{
    using UnityEngine;
    using Weapon;
    using System.Collections;

    public class MeleeActivePlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [HideInInspector] public MeleeWeapon currentMeleeWeapon;
        [SerializeField] private WeaponManager weaponManager;
        private Coroutine activeCoroutine;

        public override void OnStateEnter()
        {
            inputBufferActive = true;
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeWeapon meleeWeapon)
            {
                if (activeCoroutine == null)
                {
                    currentMeleeWeapon = meleeWeapon;
                    activeCoroutine = StartCoroutine(Active());
                }
       
            }
        }
        
        private IEnumerator Active()
        {
            currentMeleeWeapon.Active();
            currentAttack = currentMeleeWeapon.GetAttack();
            yield return new WaitForSeconds(currentAttack.activeTime);
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            activeCoroutine = null;
            playerStateInterpretor.AddState("MeleeRecoveryPlayerState", PlayerStateType.ACTION, false);
            playerStateInterpretor.Behave(currentMeleeWeapon,PlayerStateType.ACTION);
            currentAttack = null;
            currentMeleeWeapon = null;
        }

        public override void OnStateExit()
        {
            
        }
    }
}
