namespace Module.Behaviours
{
    using System;
    using Definitions;
    using UnityEngine;

    [Serializable]
    public class InstantiatedZoneModule : InstantiatedModule
    {
        protected ZoneBehaviourData Data;

        protected override void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.Data = (ZoneBehaviourData) data;
        }

        protected override void CollisionBehaviour()
        {
            Collider[] colliders = this.CheckCollisions();
            foreach (var col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(this.Data.Damages, Vector3.zero);
                
                Vector3 closestPoint = col.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, col));
            }
        }
    }
}
