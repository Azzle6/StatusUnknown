namespace Weapons
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using Inventory.Containers;
    using Inventory.Item;
    using Module;
    using Sirenix.OdinInspector;
    using UnityEngine.Serialization;

    [Serializable]
    public class WeaponData : ItemData
    {
        public override GridItemSO GridItemDefinition => this.definition;
        public WeaponDefinitionSO definition;
        
        public WeaponTriggerData[] triggerInfoData;
        
        #region CONSTRUCTOR
        public WeaponData(WeaponDefinitionSO def)
        {
            List<WeaponTriggerData> triggerInfosResult = new List<WeaponTriggerData>();
            
            foreach (WeaponTriggerDefinition trigger in def.triggers)
                triggerInfosResult.Add(new WeaponTriggerData(trigger));

            this.triggerInfoData = triggerInfosResult.ToArray();
            this.definition = def;
        }
        #endregion

        [Button("Auto-complete triggers")]
        private void RefreshWeaponTriggers()
        {
            if(this.definition == null)
                return;

            List<WeaponTriggerData> result = new List<WeaponTriggerData>();
            foreach (WeaponTriggerDefinition trigger in this.definition.triggers)
            {
                result.Add(new WeaponTriggerData(trigger));
            }

            this.triggerInfoData = result.ToArray();
        }
    }

    [Serializable]
    public struct WeaponTriggerData : IItemsDataContainer
    {
        public TriggerSO triggerType;
        public int triggerRowPosition;
        public VectorIntModuleDictionary modules;
        public ModuleCompilation compiledModules;
        
        #region CONSTRUCTOR
        public WeaponTriggerData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.triggerType = triggerDefinitionData.trigger;
            this.modules = new VectorIntModuleDictionary();
            this.triggerRowPosition = triggerDefinitionData.triggerRowPosition;
            this.compiledModules = new ModuleCompilation();
        }
        #endregion
       
        public VectorIntItemDictionary GetAllItems()
        {
            return this.modules.ToItemDictionary();
        }

        public void SaveAllItems(VectorIntItemDictionary content)
        {
            this.modules.Clear();
            foreach (var info in content)
            {
                this.modules.Add(info.Key, (ModuleData)info.Value);
            }
            this.compiledModules.CompileWeaponModules(this.triggerRowPosition, this.modules);
        }

        public void ClearData()
        {
            this.modules.Clear();
        }
    }
}
