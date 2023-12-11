namespace Inventory
{
    using System;
    using System.Collections.Generic;
    using Containers;
    using Core.Helpers;
    using Item;
    using Module;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Weapons;

    [Serializable]
    public class InventoryData : IItemsDataContainer
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
        
        [Button]
        public void AddNewItem(ItemData item)
        {
            HashSet<Vector2Int> availablePositions = this.GetAvailablePositions();
            foreach (var curPos in availablePositions)
            {
                bool isValid = true;
                foreach (var checkedPos in GridHelper.GetPositionsFromShape(item.GridItemDefinition.shape, curPos))
                {
                    if (!availablePositions.Contains(checkedPos))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    AddToList(curPos);
                    break;
                }
            }

            void AddToList(Vector2Int pos)
            {
                switch (item.GridItemDefinition.ItemType)
                {
                    case E_ItemType.MODULE:
                        this.modules.Add(pos, (ModuleData)item);
                        break;
                    case E_ItemType.WEAPON:
                        this.weapons.Add(pos, (WeaponData)item);
                        break;
                }
            }
        }

        public HashSet<Vector2Int> GetAvailablePositions()
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>();

            for (int y = 0; y < this.Shape.shapeSize.y; y++)
            {
                for (int x = 0; x < this.Shape.shapeSize.x; x++)
                {
                    Vector2Int curPos = new Vector2Int(x, y);
                    if(this.Shape.IsValidPosition(curPos))
                        result.Add(curPos);
                }
            }

            foreach (var module in this.modules)
            {
                foreach (var pos in GridHelper.GetPositionsFromShape(module.Value.GridItemDefinition.shape, module.Key))
                    result.Remove(pos);
            }

            foreach (var weapon in this.weapons)
            {
                foreach (var pos in GridHelper.GetPositionsFromShape(weapon.Value.GridItemDefinition.shape, weapon.Key))
                    result.Remove(pos);
            }

            return result;
        }

        public void ClearData()
        {
            this.modules.Clear();
            this.weapons.Clear();
        }
    }
}
