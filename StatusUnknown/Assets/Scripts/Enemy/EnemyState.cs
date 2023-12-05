using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyContext context;
    protected string name = "test";
    public Transform transform => context.transform;
    public void SetContext(EnemyContext context)
    {
        this.context = context;
        Initialize();
    }
    protected abstract void Initialize();
    public abstract void Update();

    public abstract void DebugGizmos();

}
