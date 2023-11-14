using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarmi : EnemyContext
{
    
    private void Start()
    {
        Start();
        SwitchState(new SwarmiIdle());
    }
}
