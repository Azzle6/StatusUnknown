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

        public VectorIntModuleDictionary Modules;
        public VectorIntWeaponDictionary Weapons;
        public VectorIntItemDictionary GetAllItems()
        {
            List<VectorIntItemDictionary> dictionariesToMerge = new List<VectorIntItemDictionary>()
                { this.Modules.ToItemDictionary(), this.Weapons.ToItemDictionary() };
            
            VectorIntItemDictionary result = new VectorIntItemDictionary();
            
            result.MergeMultipleDictionary(dictionariesToMerge.ToArray());
            
            return result;
        }

        public void SaveAllItems(VectorIntItemDictionary content)
        {
            this.Modules.Clear();
            this.Weapons.Clear();
            
            foreach (var info in content)
            {
                switch (info.Value.GridItemDefinition.ItemType)
                {
                    case E_ItemType.MODULE:
                        this.Modules.Add(info.Key, (ModuleData)info.Value);
                        break;
                    case E_ItemType.WEAPON:
                        this.Weapons.Add(info.Key, (WeaponData)info.Value);
                        break;
                }
            }
        }

        public void ClearData()
        {
            this.Modules.Clear();
            this.Weapons.Clear();
        }
    }
}
