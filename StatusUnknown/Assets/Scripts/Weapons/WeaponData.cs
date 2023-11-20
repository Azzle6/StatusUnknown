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
        
        public WeaponTriggerGridData[] triggerInfoData;
        
        #region CONSTRUCTOR
        public WeaponData(WeaponDefinitionSO def)
        {
            List<WeaponTriggerGridData> triggerInfosResult = new List<WeaponTriggerGridData>();
            
            foreach (WeaponTriggerDefinition trigger in def.triggers)
                triggerInfosResult.Add(new WeaponTriggerGridData(trigger));

            this.triggerInfoData = triggerInfosResult.ToArray();
            this.definition = def;
        }
        #endregion

        [Button("Auto-complete triggers")]
        private void RefreshWeaponTriggers()
        {
            if(this.definition == null)
                return;

            List<WeaponTriggerGridData> result = new List<WeaponTriggerGridData>();
            foreach (WeaponTriggerDefinition trigger in this.definition.triggers)
            {
                result.Add(new WeaponTriggerGridData(trigger));
            }

            this.triggerInfoData = result.ToArray();
        }
    }

    [Serializable]
    public struct WeaponTriggerGridData : IItemsDataContainer
    {
        public TriggerSO triggerType;
        public int triggerRowPosition;
        public VectorIntModuleDictionary modules;
        
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
        }

        public void ClearData()
        {
            this.modules.Clear();
        }

        public WeaponTriggerGridData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.triggerType = triggerDefinitionData.trigger;
            this.modules = new VectorIntModuleDictionary();
            this.triggerRowPosition = triggerDefinitionData.triggerRowPosition;
        }
    }
}
