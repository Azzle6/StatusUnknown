namespace Weapons
{
    using System;
    using System.Collections.Generic;
    using AYellowpaper.SerializedCollections;
    using Inventory.Item;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public class Weapon
    {
        public WeaponDefinitionSO definition;
        public WeaponTriggerInfoData[] triggerInfoData;
        
        #region CONSTRUCTOR
        public Weapon(WeaponDefinitionSO def)
        {
            this.definition = def;
            List<WeaponTriggerInfoData> triggerInfosResult = new List<WeaponTriggerInfoData>();
            
            foreach (WeaponTriggerDefinition trigger in def.triggers)
                triggerInfosResult.Add(new WeaponTriggerInfoData(trigger));

            this.triggerInfoData = triggerInfosResult.ToArray();
        }
        #endregion

        [Button("Auto-complete triggers")]
        private void RefreshWeaponTriggers()
        {
            if(this.definition == null)
                return;

            List<WeaponTriggerInfoData> result = new List<WeaponTriggerInfoData>();
            foreach (WeaponTriggerDefinition trigger in this.definition.triggers)
            {
                result.Add(new WeaponTriggerInfoData(trigger));
            }

            this.triggerInfoData = result.ToArray();
        }
    }

    [Serializable]
    public class WeaponTriggerInfoData
    {
        public E_TriggerType triggerType;
        [SerializedDictionary] 
        public SerializedDictionary<Vector2Int, Item> content;

        public WeaponTriggerInfoData(WeaponTriggerDefinition triggerDefinitionData)
        {
            this.triggerType = triggerDefinitionData.trigger;
            this.content = new SerializedDictionary<Vector2Int, Item>();
        }
    }
}
