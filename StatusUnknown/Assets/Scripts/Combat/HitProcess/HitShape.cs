using UnityEngine;
public abstract class HitShape 
{
    public abstract Collider[] DetectColliders(HitContext hitContext);
    public abstract Collider[] DetectColliders(Vector3 position, Quaternion rotation, LayerMask hitMask);
    public abstract void DrawGizmos(HitContext hitContext);
}
