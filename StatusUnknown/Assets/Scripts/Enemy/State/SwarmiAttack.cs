using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmiAttack : EnemyState
{
    Color colorState = Color.red;
    float attackRange => context.stats.AttackRange;
    public override void Update()
    {
        if (!CombatManager.PlayerInRange(transform.position, attackRange))
        {
            context.SwitchState(new SwarmiChase());
        }
    }

    protected override void Initialize()
    {
        context.changeColor(colorState);
    }
}
