namespace Module
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;

    public class ModuleItemView : ItemView
    {
        public ModuleItemView(ModuleData dataItem, Vector2Int gridPosition, GridView gridView) : base(dataItem, gridPosition,
            gridView)
        {
            this.ModuleDataItem = dataItem;
        }

        public ModuleData ModuleDataItem;

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for module.");
        }
    }
}
