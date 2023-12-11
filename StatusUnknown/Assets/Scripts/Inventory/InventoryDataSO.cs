namespace Inventory
{
    using System.Collections.Generic;
    using Containers;
    using Core.Helpers;
    using Item;
    using Module;
    using UnityEngine;
    using Weapons;

    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class InventoryDataSO : ScriptableObject, IItemsDataContainer
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        public VectorIntModuleDictionary modules;
        public VectorIntWeaponDictionary weapons;
        public ContainedItemInfo[] GetAllItems()
        {
            List<ContainedItemInfo> result = new List<ContainedItemInfo>();

            foreach (var module in this.modules)
                result.Add(new ContainedItemInfo(module.Key, module.Value));
            
            foreach (var weapon in this.weapons)
                result.Add(new ContainedItemInfo(weapon.Key, weapon.Value));
            
            return result.ToArray();
        }

        public void SaveAllItems(ContainedItemInfo[] content)
        {
            this.modules.Clear();
            this.weapons.Clear();
            
            foreach (var info in content)
            {
                switch (info.ItemData.GridItemDefinition.ItemType)
                {
                    case E_ItemType.MODULE:
                        this.modules.Add(info.Coordinates, (ModuleData)info.ItemData);
                        break;
                    case E_ItemType.WEAPON:
                        this.weapons.Add(info.Coordinates, (WeaponData)info.ItemData);
                        break;
                }
            }
        }

        public void ClearData()
        {
            this.modules.Clear();
            this.weapons.Clear();
        }
    }
}
