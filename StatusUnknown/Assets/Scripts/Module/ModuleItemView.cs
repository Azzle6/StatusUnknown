namespace Module
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;

    public class ModuleItemView : ItemView
    {
        public ModuleItemView(Module item, Vector2Int gridPosition, GridView gridView) : base(item, gridPosition,
            gridView)
        {
            this.ModuleItem = item;
        }

        public Module ModuleItem;

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for module.");
        }
    }
}
