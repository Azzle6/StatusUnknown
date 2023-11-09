using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmiAttack : EnemyState
{
    Color colorState = Color.red;
    float attackDuration;
    float attackRange => context.stats.AttackRange;
    public override void Update()
    {
        attackDuration -= Time.deltaTime;
        if (!CombatManager.PlayerInRange(transform.position, attackRange) && attackDuration < 0)
        {
            context.SwitchState(new SwarmiChase());
        }
    }

    protected override void Initialize()
    {
        attackDuration = context.stats.AttackDuration;
        context.changeColor(colorState);
    }
}
