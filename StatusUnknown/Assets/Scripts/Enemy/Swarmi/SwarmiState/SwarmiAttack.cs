using UnityEngine;
namespace Enemy.Swarmi
{
    public class SwarmiAttack : EnemyState
    {
        Color colorState = Color.red;

        float attackRange => context.stats.AttackRange;
        float attackDuration;
        float attackCooldown => ((EnemySwarmi)context).attackCoolDown;

        public override void Update()
        {
            attackDuration -= Time.deltaTime;
            if (CombatManager.playerTransform != null)
            {
                //context.RotateTowards(CombatManager.playerTransform.position - transform.position, 180);
            }
            if (attackDuration < 0)
            {
                context.SwitchState(new SwarmiChase());
            }
        }

        protected override void Initialize()
        {

            ((EnemySwarmi)context).attackCoolDown = context.stats.AttackCooldown;
            attackDuration = context.stats.AttackDuration;
            context.PlayAnimation("SwarmiAttack");
        }

    }
}