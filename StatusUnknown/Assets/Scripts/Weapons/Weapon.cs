namespace Weapons
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using Inventory.Item;
    using Module;
    using Sirenix.OdinInspector;

    [Serializable]
    public class Weapon : Item
    {
        public override GridItemSO GridItemDefinition => this.definition;
        public WeaponDefinitionSO definition;
        
        public WeaponTriggerGridData[] triggerInfoData;
        
        #region CONSTRUCTOR
        public Weapon(WeaponDefinitionSO def)
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
    public struct WeaponTriggerGridData
    {
        public TriggerSO triggerType;
        public int triggerRowPosition;
        public VectorIntItemDictionary content;

        public WeaponTriggerGridData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.triggerType = triggerDefinitionData.trigger;
            this.content = new VectorIntItemDictionary();
            this.triggerRowPosition = triggerDefinitionData.triggerRowPosition;
        }
    }
}
