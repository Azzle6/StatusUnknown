namespace Inventory.Item
{
    using System;

    [Serializable]
    public class Item
    {
        public GridItemSO gridItemDefinition;
        public Item(GridItemSO gridItemDefinition)
        {
            this.gridItemDefinition = gridItemDefinition;
        }
    }
}
