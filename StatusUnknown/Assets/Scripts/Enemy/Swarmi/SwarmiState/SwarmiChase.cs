using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorField;

public class SwarmiChase : EnemyState
{

    float chaseSpeed => context.stats.chaseStrength;
    float attackRange => context.stats.AttackRange;
    float hoverOffset = 0.5f;

    float angleSpeed = 180;
    bool inRangeAttack;
    //Debug
    Color stateColor = Color.yellow;
    public override void Update()
    {
        ((EnemySwarmi)context).attackCoolDown -= Time.deltaTime; // TODO: find a way to implement context inheritance in state
        bool inRangeAttack = CombatManager.PlayerInRange(transform.position, attackRange);

        if (inRangeAttack && ((EnemySwarmi)context).attackCoolDown < 0)
            context.SwitchState(new SwarmiAttack());

        if (!CombatManager.PlayerInRange(transform.position, context.stats.AggroRange))
            context.SwitchState(new SwarmiIdle());

    }

    public override void DebugGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(context.transform.position, attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(context.transform.position, context.stats.AggroRange);
    }

    public override void FixedUpdate()
    {
        Node node = VectorFieldNavigator.WorldPositiondToNode(context.transform.position, 4);
        if (node != null && !inRangeAttack)
        {
            //Vector3 targetPosition = node.Position + node.targetDirection + Vector3.up;

            Vector3 targetVector = node.targetDirection;

            context.AddForce(targetVector * chaseSpeed);
            Vector3 force = Vector3.down * chaseSpeed;
            context.AddForce(force * Mathf.Max(Vector3.Distance(transform.position, node.Position) - hoverOffset, 0));
            Debug.DrawLine(transform.position, node.Position);
        }
        context.AddForce(context.GetAvoidance());
        if (context.Velocity != Vector3.zero && !inRangeAttack)
            context.RotateTowards(context.Velocity, angleSpeed);
        if (CombatManager.playerTransform != null && inRangeAttack)
            context.RotateTowards(CombatManager.playerTransform.position - transform.position, angleSpeed);
    }
}
