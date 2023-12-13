using UnityEngine;
public abstract class HitShape
{
    protected float Scale = 1;
    public abstract Collider[] DetectColliders(HitContext hitContext);
    public abstract Collider[] DetectColliders(Vector3 position, Quaternion rotation, LayerMask hitMask);
    public abstract void DrawGizmos(HitContext hitContext);
}
