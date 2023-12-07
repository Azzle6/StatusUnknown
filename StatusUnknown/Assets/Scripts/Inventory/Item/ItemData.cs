namespace Inventory.Item
{
    using System;

    [Serializable]
    public abstract class ItemData
    {
        public abstract GridItemSO GridItemDefinition { get; }
    }
}
