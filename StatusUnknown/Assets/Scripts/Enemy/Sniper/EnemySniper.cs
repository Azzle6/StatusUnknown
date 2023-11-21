using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniper : EnemyContext
{
    public SniperStats sniperStats;
    public LayerMask obstacleMask;
    public override EnemyStats stats => sniperStats;
    private void Start()
    {
        SwitchState(new SniperIdle());
    }
}
