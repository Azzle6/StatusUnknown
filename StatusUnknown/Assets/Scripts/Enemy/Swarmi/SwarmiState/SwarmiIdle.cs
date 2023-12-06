using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmiIdle : EnemyState
{
    Color stateColor = Color.green;

    public override void DebugGizmos()
    {
        Gizmos.DrawWireSphere(context.transform.position, context.stats.AggroRange);
    }

    public override void Update()
    {
        ((EnemySwarmi)context).attackCoolDown -= Time.deltaTime;
        if (CombatManager.PlayerInRange(transform.position, context.stats.AggroRange))
            context.SwitchState(new SwarmiChase());
    }
}
