namespace Combat.HitProcess
{
    using System;
    using UnityEngine;
    [Serializable]
    public class HitSphere : HitShape
    {
        [SerializeField] public float radius = 1;
        public override Collider[] DetectColliders(HitContext hitContext)
        {
            return Physics.OverlapSphere(hitContext.transform.position, this.radius,hitContext.hitMask);
        }

        public override Collider[] DetectColliders(Vector3 position, Quaternion rotation, LayerMask hitMask)
        {
            return Physics.OverlapSphere(position, this.radius, hitMask);
        }

        public override void DrawGizmos(HitContext hitContext)
        {
            Gizmos.DrawWireSphere(hitContext.transform.position, this.radius);
        }
    }
}
