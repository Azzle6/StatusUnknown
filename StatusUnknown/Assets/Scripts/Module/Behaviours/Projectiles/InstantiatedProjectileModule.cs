namespace Module.Behaviours.Projectiles
{
    using Definitions;
    using UnityEngine;

    public class InstantiatedProjectileModule : InstantiatedModule
    {
        protected ProjectileBehaviourData Data;

        public void Init(ProjectileBehaviourData data, CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            base.BaseInit(compiledModule, info);
            this.Data = data;
        }

        protected void CheckCollisions()
        {
            Debug.Log("Check collisions");
        }
    }
}
