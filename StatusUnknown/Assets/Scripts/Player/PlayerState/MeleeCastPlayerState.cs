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
            currentMeleeWeapon = (MeleeWeapon) weaponManager.GetCurrentWeapon();
            currentMeleeWeapon.Cast();
        }

       
        public override void Behave<T>(T x)
        {
            if (x is MeleeWeapon weapon)
            {
                Debug.Log("Cast received");
                currentMeleeWeapon = weapon;
                if ((castCoroutine == null) && (!playerStateInterpretor.CheckState(PlayerStateType.ACTION,"MeleeBuildUpPlayerState")))
                {
                    Debug.Log("Cast coroutine started");
                    castCoroutine = StartCoroutine(Cast());
                }
            }
        }

        
        
        private IEnumerator Cast()
        {
            //launch the cast animation
            //need to match animation length
                if(currentMeleeWeapon == default)
                    yield break;
                
            currentAttack = currentMeleeWeapon.GetAttack();
            yield return new WaitForSeconds(currentAttack.castTime);
          
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            playerStateInterpretor.AddState("MeleeBuildUpPlayerState", PlayerStateType.ACTION, true);
            currentAttack = null;
            playerStateInterpretor.Behave(currentMeleeWeapon,PlayerStateType.ACTION);
            castCoroutine = null;
            currentMeleeWeapon = null;
        }

            
        
        public override void OnStateExit()
        {
            
        }
    }
}
