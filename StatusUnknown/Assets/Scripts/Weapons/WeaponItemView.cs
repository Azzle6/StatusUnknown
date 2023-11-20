namespace Weapons
{
    using Inventory.Grid;
    using Inventory.Item;
    using UnityEngine;
    
    public class WeaponItemView : ItemView
    {
        public WeaponItemView(WeaponData itemData, Vector2Int gridPosition, GridView gridView) : base(itemData, gridPosition,
            gridView)
        {
            this.WeaponItemData = itemData;
        }

        public WeaponData WeaponItemData;

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for weapon.");
        }
    }
}
