using UnityEngine;
public abstract class HitShape
{
    [HideInInspector] public float Scale = 1;
    public abstract Collider[] DetectColliders(HitContext hitContext);
    public abstract Collider[] DetectColliders(Vector3 position, Quaternion rotation, LayerMask hitMask);
    public abstract void DrawGizmos(HitContext hitContext);
    public abstract void DrawGizmos(Transform transform);
}
