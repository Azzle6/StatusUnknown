using UnityEngine;
[System.Serializable]
public class HitBox : HitShape
{
    public Vector3 size = Vector3.one;
    Mesh debugMesh;
    public override Collider[] DetectColliders(HitContext hitContext)
    {
        return Physics.OverlapBox(hitContext.transform.position, this.size * (0.5f * this.Scale), hitContext.transform.rotation, hitContext.hitMask);
    }

    public override Collider[] DetectColliders(Vector3 position, Quaternion rotation, LayerMask hitMask)
    {
        return Physics.OverlapBox(position, size * (0.5f * this.Scale), rotation,hitMask);
    }

    public override void DrawGizmos(HitContext hitContext)
    {
        if (debugMesh == null)
            debugMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        Gizmos.DrawWireMesh(debugMesh, hitContext.transform.position, hitContext.transform.rotation, size * this.Scale);
    }

    public override void DrawGizmos(Transform transform)
    {
        if (debugMesh == null)
            debugMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        Gizmos.DrawWireMesh(debugMesh, transform.position, transform.rotation, size * this.Scale);
    }
}
