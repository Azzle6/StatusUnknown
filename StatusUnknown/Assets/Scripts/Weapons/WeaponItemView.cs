namespace Weapons
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;
    
    public class WeaponItemView : ItemView
    {
        public WeaponItemView(Item item, Vector2Int gridPosition, GridView gridView) : base(item, gridPosition, gridView)
        { }

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for weapon.");
        }
    }
}
