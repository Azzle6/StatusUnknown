namespace Inventory
{
    using UnityEngine;
    using Weapons;

    [CreateAssetMenu(menuName = "CustomAssets/Data/PlayerInventory")]
    public class PlayerInventorySO : ScriptableObject
    {
        public InventoryDataSO inventory;
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
                            return triggerData;
                        }
                    }
                }
            }
            return null;
        }
    }
}