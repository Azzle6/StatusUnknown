namespace Module.Behaviours
{
    using Core;
    using Definitions;
    using Projectiles;
    using Unity.VisualScripting;
    using UnityEngine;

    public class ModuleBehaviourHandler : MonoSingleton<ModuleBehaviourHandler>
    {
        public void GetModuleBehaviourToInstantiate(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            ModuleDefinitionSO moduleDefinition = compiledModule.module.definition;
            
            if (moduleDefinition.ModuleType != E_ModuleType.BEHAVIOUR)
            {
                Debug.LogWarning($"Trying to instantiate {moduleDefinition.ModuleType} which is not a behaviour module. Self destroy.");
                return;
            }
            
            BehaviourModuleDefinitionSO behaviourDefinition = moduleDefinition as BehaviourModuleDefinitionSO;
            
            switch (behaviourDefinition.BehaviourData)
            {
                case ProjectileBehaviourData data:
                    var scriptClass = data.projectileBehaviour.GetClass();
                    InstantiatedProjectileModule module = new GameObject("module", scriptClass).GetComponent<InstantiatedProjectileModule>();
                    module.AddComponent<MeshFilter>().mesh = data.mesh;
                    module.AddComponent<MeshRenderer>().material = data.material;
                    module.Init(data, compiledModule, info);
                    break;
                case ZoneBehaviourData data:
                    
                    break;
                case DropBehaviourData data:
                    
                    break;
            }
        }
    }
}
