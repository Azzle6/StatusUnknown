using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSphere : HitShape
{
    [SerializeField] float radius = 1;
    public override Collider[] DetectColliders(HitContext hitContext)
    {
        return Physics.OverlapSphere(hitContext.transform.position, radius,hitContext.hitMask);
    }

    public override void DrawGizmos(HitContext hitContext)
    {
        Gizmos.DrawWireSphere(hitContext.transform.position, radius);
    }
}
