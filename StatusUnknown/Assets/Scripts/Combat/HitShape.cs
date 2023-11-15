using UnityEngine;
public abstract class HitShape 
{
    public abstract Collider[] DetectColliders(HitContext hitContext);
    public abstract void DrawGizmos(HitContext hitContext);
}
