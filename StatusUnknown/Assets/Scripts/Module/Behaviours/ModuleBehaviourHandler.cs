namespace Module.Behaviours
{
    using Definitions;
    using UnityEngine;
    public static class ModuleBehaviourHandler
    {
        public static void InstantiateModuleBehaviour(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            ModuleDefinitionSO moduleDefinition = compiledModule.module.definition;
            
            if (moduleDefinition.ModuleType != E_ModuleType.BEHAVIOUR)
            {
                Debug.LogWarning($"Trying to instantiate {moduleDefinition.ModuleType} which is not a behaviour module. Self destroy.");
                return;
            }
            
            BehaviourModuleDefinitionSO behaviourDefinition = moduleDefinition as BehaviourModuleDefinitionSO;
            
            switch (behaviourDefinition.Behaviour)
            {
                case ProjectileBehaviourData data:
                    
                    break;
                case ZoneBehaviourData data:
                    
                    break;
                case DropBehaviourData data:
                    
                    break;
            }
        }
    }

    public struct InstantiatedModuleInfo
    {
        public Vector3 TriggeredPosition;
        public Vector3 Direction;

        public InstantiatedModuleInfo(Vector3 triggeredPosition, Vector3 direction)
        {
            this.TriggeredPosition = triggeredPosition;
            this.Direction = direction;
        }
    }
}
