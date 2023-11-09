namespace Inventory
{
    using System;

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
