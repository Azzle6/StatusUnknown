namespace Inventory
{
    using System;
    using UnityEngine;
    using UnityEngine.UIElements;

    [Serializable]
    public class Item
    {
        public ItemSO itemDefinition;
        public Vector2Int gridPosition;

        public Item(ItemSO itemDefinition, Vector2Int gridPosition)
        {
            this.itemDefinition = itemDefinition;
            this.gridPosition = gridPosition;
        }
    }
}
