using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorField;

public class SwarmiChase : EnemyState
{
    float chaseSpeed => context.stats.chaseStrength;
    float attackRange => context.stats.AttackRange;
    float attackDuration;
    //Debug
    Color stateColor = Color.yellow;
    public override void Update()
    {
        Node node = VectorFieldNavigator.WorlPositiondToNode(context.transform.position, 4);
        if (node != null)
        {
            //Vector3 targetPosition = node.Position + node.targetDirection + Vector3.up;
            Vector3 targetVector = node.targetDirection;
            context.AddForce(targetVector * chaseSpeed);
            context.AddForce(((node.Position + Vector3.up) - transform.position) * chaseSpeed * 0.5f) ;
        }

        if (CombatManager.PlayerInRange(transform.position, attackRange))
            context.SwitchState(new SwarmiAttack());

        if (!CombatManager.PlayerInRange(transform.position, context.stats.AggroRange))
            context.SwitchState(new SwarmiChase());

    }
    protected override void Initialize()
    {
        
        context.changeColor(stateColor);
    }
}
