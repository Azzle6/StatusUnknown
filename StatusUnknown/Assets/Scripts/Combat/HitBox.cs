
using UnityEngine;
[System.Serializable]
public class HitBox : HitShape
{
    [SerializeField] Vector3 size = Vector3.one;
    Mesh debugMesh;
    public override Collider[] DetectColliders(HitContext hitContext)
    {
        return Physics.OverlapBox(hitContext.transform.position, size * 0.5f, hitContext.transform.rotation, hitContext.hitMask);
    }

    public override void DrawGizmos(HitContext hitContext)
    {
        if (debugMesh == null)
            debugMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        Gizmos.DrawWireMesh(debugMesh, hitContext.transform.position, hitContext.transform.rotation, size);
    }
}
