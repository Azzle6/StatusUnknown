namespace Module
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;

    public class ModuleItemView : ItemView
    {
        public ModuleItemView(Item item, Vector2Int gridPosition, GridView gridView) : base(item, gridPosition,
            gridView)
        { }

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for module.");
        }
    }
}
