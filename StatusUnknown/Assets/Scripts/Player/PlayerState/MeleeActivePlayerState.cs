

using Input;

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
        [SerializeField] private LayerMask enemyLayer;
        private Coroutine activeCoroutine;
        private Collider[] enemies;

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
            DetectAndDamage();
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

        private void DetectAndDamage()
        {
            enemies = Physics.OverlapSphere(transform.position, currentAttack.attackLength, enemyLayer);

            if (enemies.Length != default)
            {
                StartCoroutine(GamePadRumbleManager.ExecuteRumbleWithTime(currentAttack.rumbleOnHit, true));
            }
            
            foreach (Collider enemy in enemies)
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToEnemy);
                
                if (angle < currentAttack.attackAngle)
                {
                    if (enemy.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(currentAttack.attackDamage, transform.forward * currentAttack.attackKnockback);
                    }
                }
            }
        }
        
    }
}
