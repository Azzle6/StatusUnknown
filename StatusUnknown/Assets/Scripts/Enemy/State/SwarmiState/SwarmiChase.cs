using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorField;

public class SwarmiChase : EnemyState
{
    float chaseSpeed => context.stats.chaseStrength;
    float attackRange => context.stats.AttackRange;
    float attackDuration;
    float hoverOffset = 0.5f;

    float angleSpeed = 180;
    //Debug
    Color stateColor = Color.yellow;
    public override void Update()
    {
        
        Node node = VectorFieldNavigator.WorldPositiondToNode(context.transform.position, 4);
        if (node != null)
        {
            //Vector3 targetPosition = node.Position + node.targetDirection + Vector3.up;
            
            Vector3 targetVector = node.targetDirection;

            context.AddForce(targetVector * chaseSpeed);
            Vector3 force = Vector3.down *2* chaseSpeed;
            context.AddForce(force * Mathf.Max(Vector3.Distance(transform.position, node.Position) - hoverOffset,0));
            Debug.DrawLine(transform.position, node.Position);
        }
        context.AddForce(context.GetAvoidance());
        context.RotateTowards(context.Velocity, angleSpeed);

        if (CombatManager.PlayerInRange(transform.position, attackRange))
            context.SwitchState(new SwarmiAttack());

        if (!CombatManager.PlayerInRange(transform.position, context.stats.AggroRange))
            context.SwitchState(new SwarmiChase());

    }

    protected override void Initialize()
    {
        
    }
}
