
namespace Enemy.Sniper
{
    using Enemy;
    using System.Collections.Generic;
    using UnityEngine;
    using VectorField;
    public class SniperIdle : EnemyState
    {
        const int obstructLayer = 8; // obstaclelayer
        EnemySniper currentContext => context as EnemySniper;
        SniperStats sniperStats => currentContext.sniperStats;


        public override void Update()
        {
            currentContext.attackCooldown -= Time.deltaTime;
            currentContext.tpCooldown -= Time.deltaTime;
            bool playerInView = CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask);

            if (playerInView && currentContext.attackCooldown < 0)
                currentContext.SwitchState(new SniperAttack());

            if (currentContext.tpCooldown < 0)
                currentContext.SwitchState(new SniperMove());

            if (CombatManager.playerTransform != null && playerInView)
                transform.forward = CombatManager.playerTransform.position - transform.position;
        }
      

        protected override void Initialize()
        {
            
        }

    }
}