namespace Module.Behaviours.Projectiles
{
    using Definitions;
    using UnityEngine;

    public class InstantiatedProjectileModule : InstantiatedModule
    {
        protected ProjectileBehaviourData Data;

        public void Init(ProjectileBehaviourData data, CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            BaseInit(compiledModule, info);
            this.Data = data;
        }

        protected Collider[] CheckCollisions()
        {
            Collider[] result = this.Data.shape.DetectColliders(this.transform.position, this.transform.rotation,
                this.Data.layerMask);
            
            foreach (var hitItem in result)
            {
                Vector3 closestPoint = hitItem.ClosestPoint(transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, closestPoint - transform.position));
            }

            return result;
        }
    }
}
