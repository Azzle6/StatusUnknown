namespace Weapons
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;
    
    public class WeaponItemView : ItemView
    {
        public WeaponItemView(Weapon item, Vector2Int gridPosition, GridView gridView) : base(item, gridPosition,
            gridView)
        {
            this.WeaponItem = item;
        }

        public Weapon WeaponItem;

        protected override void GenerateCustomView()
        {
            Debug.Log("Generate custom view for weapon.");
        }
    }
}
