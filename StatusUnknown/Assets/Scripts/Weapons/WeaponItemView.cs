namespace Weapons
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;
    
    public class WeaponItemView : ItemView
    {
        public WeaponItemView(WeaponData dataItem, Vector2Int gridPosition, GridView gridView) : base(dataItem, gridPosition,
            gridView)
        {
            this.WeaponDataItem = dataItem;
        }

        public WeaponData WeaponDataItem;

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for weapon.");
        }
    }
}
