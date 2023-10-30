namespace Inventory
{
    using System;
    using UnityEngine;
    using UnityEngine.UIElements;

    [Serializable]
    public class Item
    {
        public ItemSO itemDefinition;

        public Item(ItemSO itemDefinition)
        {
            this.itemDefinition = itemDefinition;
        }
    }
}
