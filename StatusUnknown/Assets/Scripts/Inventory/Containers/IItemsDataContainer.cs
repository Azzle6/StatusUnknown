namespace Inventory.Containers
{
    using System;
    using Item;
    using UnityEngine;

    public interface IItemsDataContainer
    {
        public ContainedItemInfo[] GetAllItems();

        public void SaveAllItems(ContainedItemInfo[] content);

        public void ClearData();
    }

    [Serializable]
    public struct ContainedItemInfo
    {
        public Vector2Int Coordinates;
        public ItemData ItemData;

        public ContainedItemInfo(Vector2Int coordinates, ItemData itemData)
        {
            this.Coordinates = coordinates;
            this.ItemData = itemData;
        }
    }
}
