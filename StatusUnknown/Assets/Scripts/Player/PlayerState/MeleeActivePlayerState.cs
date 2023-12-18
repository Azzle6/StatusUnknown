

using Input;

namespace Player
{
    using UnityEngine;
    using Weapon;
    using System.Collections;
    using Core.SingletonsSO;
    using Module.Behaviours;
    using Weapons;

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
            E_WeaponOutput output;
            switch (this.currentMeleeWeapon.comboIndex)
            {
                case 0:
                    output = E_WeaponOutput.ON_SPAWN_FIRST_ATTACK;
                    break;
                case 1:
                    output = E_WeaponOutput.ON_SPAWN_SECOND_ATTACK;
                    break;
                case 2:
                    output = E_WeaponOutput.ON_SPAWN_THIRD_ATTACK;
                    break;
                default:
                    output = E_WeaponOutput.ON_SPAWN_FIRST_ATTACK;
                    break;
            }
            ModuleBehaviourHandler.Instance.CastModule(this.currentMeleeWeapon.inventory, this.currentMeleeWeapon.weaponDefinition, output, transform.position + this.currentAttack.attackLength * transform.forward, transform.rotation, null);
            DetectAndDamage();
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            activeCoroutine = null;
            playerStateInterpretor.AddState("MeleeRecoveryPlayerState", PlayerStateType.ACTION, true);
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

            for (var index = 0; index < this.enemies.Length; index++)
            {
                var enemy = this.enemies[index];
                Vector3 directionToEnemy = (enemy.transform.position - this.transform.position).normalized;
                float angle = Vector3.Angle(this.transform.forward, directionToEnemy);

                if (angle < this.currentAttack.attackAngle)
                {
                    if (enemy.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(this.currentAttack.attackDamage,
                            this.transform.forward * this.currentAttack.attackKnockback);

                        Quaternion baseRotation =
                            Quaternion.AngleAxis(Quaternion.Angle(transform.rotation, enemy.transform.rotation),
                                transform.up);
                        Quaternion finalRotation = Quaternion.LookRotation(baseRotation * transform.forward);
                        if (index == 0)
                        {
                            E_WeaponOutput output;
                            switch (this.currentMeleeWeapon.comboIndex)
                            {
                                case 0:
                                    output = E_WeaponOutput.ON_HIT_FIRST_ATTACK;
                                    break;
                                case 1:
                                    output = E_WeaponOutput.ON_HIT_SECOND_ATTACK;
                                    break;
                                case 2:
                                    output = E_WeaponOutput.ON_HIT_THIRD_ATTACK;
                                    break;
                                default:
                                    output = E_WeaponOutput.ON_HIT_FIRST_ATTACK;
                                    break;
                            }

                            ModuleBehaviourHandler.Instance.CastModule(this.currentMeleeWeapon.inventory,
                                this.currentMeleeWeapon.weaponDefinition, output, enemy.transform.position,
                                finalRotation, enemy);
                        }

                        ModuleBehaviourHandler.Instance.CastModule(this.currentMeleeWeapon.inventory,
                            this.currentMeleeWeapon.weaponDefinition, E_WeaponOutput.ON_HIT, enemy.transform.position,
                            finalRotation, enemy);
                    }
                }
            }
        }
    }
}
