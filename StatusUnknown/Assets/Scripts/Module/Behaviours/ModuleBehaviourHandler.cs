namespace Module.Behaviours
{
    using Core;
    using Definitions;
    using UnityEngine;
    using Weapons;

    public class ModuleBehaviourHandler : MonoSingleton<ModuleBehaviourHandler>
    {
        public void InstantiateModuleBehaviour(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            if (compiledModule == null)
            {
                Debug.Log("Try to trigger null module.");
                return;
            }
            
            ModuleDefinitionSO moduleDefinition = compiledModule.module.definition;
            
            if (moduleDefinition.ModuleType != E_ModuleType.BEHAVIOUR)
            {
                Debug.LogWarning($"Trying to instantiate {moduleDefinition.ModuleType} which is not a behaviour module. Self destroy.");
                return;
            }
            
            BehaviourModuleDefinitionSO behaviourDefinition = moduleDefinition as BehaviourModuleDefinitionSO;
            
            this.InstantiateModule(compiledModule, info, behaviourDefinition.BehaviourData);
        }

        private void InstantiateModule(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            ElementPositionInfo[] positions =
                data.InstantiationRule.GetInstantiationInfo(info.TriggeredPosition, info.Rotation, data.Quantity);
            
            for (int i = 0; i < data.Quantity; i++)
            {
                new GameObject("module", data.Behaviour.ScriptType).GetComponent<InstantiatedModule>().Init(compiledModule, new InstantiatedModuleInfo(positions[i].Position, positions[i].Rotation, info.LastHit), data);
            }
        }
    }
}