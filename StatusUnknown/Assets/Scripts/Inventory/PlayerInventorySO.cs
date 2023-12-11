namespace Inventory
{
    using UnityEngine;
    using Weapons;

    [CreateAssetMenu(menuName = "CustomAssets/Data/PlayerInventory")]
    public class PlayerInventorySO : ScriptableObject
    {
        public InventoryData InventoryData;
        public WeaponData[] equippedWeaponsData;

        public WeaponTriggerData GetWeaponTriggerData(WeaponDefinitionSO weaponDefinition, E_WeaponOutput output)
        {
            foreach (var weapon in this.equippedWeaponsData)
            {
                if(weapon.definition == weaponDefinition)
                {
                    foreach (var triggerData in weapon.triggerInfoData)
                    {
                        if (triggerData.weaponTriggerType == output)
                        {
                            if(triggerData.compiledModules.FirstModule == null)
                                triggerData.compiledModules.CompileWeaponModules(triggerData.triggerRowPosition, triggerData.modules);
                            return triggerData;
                        }
                    }
                }
            }
            return null;
        }
    }
}