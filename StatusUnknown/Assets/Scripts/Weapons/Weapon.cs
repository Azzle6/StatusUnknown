namespace Weapons
{
    using System;
    using System.Collections.Generic;
    using AYellowpaper.SerializedCollections;
    using Inventory.Item;
    using Module;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class Weapon
    {
        public WeaponDefinitionSO definition;
        public WeaponTriggerGridData[] triggerInfoData;
        
        #region CONSTRUCTOR
        public Weapon(WeaponDefinitionSO def)
        {
            this.definition = def;
            List<WeaponTriggerGridData> triggerInfosResult = new List<WeaponTriggerGridData>();
            
            foreach (WeaponTriggerDefinition trigger in def.triggers)
                triggerInfosResult.Add(new WeaponTriggerGridData(trigger));

            this.triggerInfoData = triggerInfosResult.ToArray();
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
        [SerializedDictionary] 
        public SerializedDictionary<Vector2Int, Item> content;

        public WeaponTriggerGridData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.triggerType = triggerDefinitionData.trigger;
            this.content = new SerializedDictionary<Vector2Int, Item>();
            this.triggerRowPosition = triggerDefinitionData.triggerRowPosition;
        }
    }
}
