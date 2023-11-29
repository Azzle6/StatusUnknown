namespace Module.Behaviours
{
    using Core;
    using Definitions;
    using UnityEngine;

    public class ModuleBehaviourHandler : MonoSingleton<ModuleBehaviourHandler>
    {
        public void InstantiateModuleBehaviour(CompiledModule compiledModule, InstantiatedModuleInfo info)
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
                    this.InstantiateProjectile(compiledModule, info, data);
                    break;
                case ZoneBehaviourData data:
                    
                    break;
                case DropBehaviourData data:
                    
                    break;
            }
        }

        private void InstantiateProjectile(CompiledModule compiledModule, InstantiatedModuleInfo info, ProjectileBehaviourData data)
        {
            ElementPositionInfo[] positions =
                data.InstantiationRule.GetInstantiationInfo(info.TriggeredPosition, info.Direction, data.quantity);
            
            for (int i = 0; i < data.quantity; i++)
            {
                var scriptClass = data.Behaviour.GetClass();
                InstantiatedProjectileModule module = new GameObject("module", scriptClass).GetComponent<InstantiatedProjectileModule>();
                module.Init(data, compiledModule, new InstantiatedModuleInfo(positions[i].Position, positions[i].Rotation));
            }
        }
    }
}