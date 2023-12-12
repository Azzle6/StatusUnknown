namespace Module.Behaviours
{
    using Core;
    using Definitions;
    using Inventory;
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
        
        public void CastModule(WeaponTriggerData data, Transform spawnPoint)
        {
            if(data == null || data.compiledModules.FirstModule == null)
                return;
            
            Instance.InstantiateModuleBehaviour(
                data.compiledModules.FirstModule,
                new InstantiatedModuleInfo(spawnPoint.position, spawnPoint.rotation));
        }
        
        public void CastModule(PlayerInventorySO inventory, WeaponDefinitionSO weaponDef, E_WeaponOutput trigger, Transform spawnPoint)
        {
            WeaponTriggerData data = inventory.GetWeaponTriggerData(weaponDef, trigger);
            CastModule(data, spawnPoint);
        }
        
        public void CastModule(WeaponTriggerData data, Vector3 spawnPoint, Quaternion rotation)
        {
            if(data == null || data.compiledModules.FirstModule == null)
                return;
            
            Instance.InstantiateModuleBehaviour(
                data.compiledModules.FirstModule,
                new InstantiatedModuleInfo(spawnPoint, rotation));
        }
        
        public void CastModule(PlayerInventorySO inventory, WeaponDefinitionSO weaponDef, E_WeaponOutput trigger, Vector3 spawnPoint, Quaternion rotation)
        {
            WeaponTriggerData data = inventory.GetWeaponTriggerData(weaponDef, trigger);
            CastModule(data, spawnPoint, rotation);
        }
    }
}