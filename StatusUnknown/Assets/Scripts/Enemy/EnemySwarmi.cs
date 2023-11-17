using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarmi : EnemyContext
{
    
    private void Start()
    {
        SwitchState(new SwarmiIdle());
    }
    
 
}
