namespace Weapons
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using Inventory.Containers;
    using Inventory.Item;
    using Module;
    using Module.Definitions;
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
    public class WeaponTriggerData : IItemsDataContainer
    {
        public E_WeaponOutput weaponTriggerType;
        public int triggerRowPosition;
        public VectorIntModuleDictionary modules;
        public ModuleCompilation compiledModules;
        
        #region CONSTRUCTOR
        public WeaponTriggerData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.weaponTriggerType = triggerDefinitionData.weaponTrigger;
            this.modules = new VectorIntModuleDictionary();
            this.triggerRowPosition = triggerDefinitionData.triggerRowPosition;
            this.compiledModules = new ModuleCompilation();
        }
        #endregion
       
        public ContainedItemInfo[] GetAllItems()
        {
            List<ContainedItemInfo> result = new List<ContainedItemInfo>();
            foreach (var module in this.modules)
                result.Add(new ContainedItemInfo(module.Key, module.Value));
            
            return result.ToArray();
        }

        public void SaveAllItems(ContainedItemInfo[] content)
        {
            this.modules.Clear();
            foreach (var info in content)
                this.modules.Add(info.Coordinates, (ModuleData)info.ItemData);
            
            this.compiledModules.CompileWeaponModules(this.triggerRowPosition, this.modules);
        }

        public void ClearData()
        {
            this.modules.Clear();
        }
    }
}
