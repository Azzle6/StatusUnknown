namespace Inventory.Item
{
    using System;

    [Serializable]
    public abstract class Item
    {
        public abstract GridItemSO GridItemDefinition { get; }
    }
}
