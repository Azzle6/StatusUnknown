namespace Inventory.Item
{
    using UnityEngine;

    public abstract class GridItemSO : ScriptableObject
    {
        public abstract E_ItemType ItemType { get; }
        public string itemName;
        public Shape shape;
        public Sprite view;
    }

    public enum E_ItemType
    {
        MODULE,
        WEAPON
    }
}
