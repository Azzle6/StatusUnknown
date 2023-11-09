using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmiIdle : EnemyState
{
    Color stateColor = Color.green;
    public override void Update()
    {
        if (CombatManager.PlayerInRange(transform.position, context.stats.AggroRange))
            context.SwitchState(new SwarmiChase());
    }

    protected override void Initialize()
    {
        context.changeColor(stateColor);
    }
}
