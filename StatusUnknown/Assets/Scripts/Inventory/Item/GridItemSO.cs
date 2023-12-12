namespace Inventory.Item
{
    using UnityEngine;

    public abstract class GridItemSO : ScriptableObject
    {
        public abstract E_ItemType ItemType { get; }
        public string itemName;
        public string description;
        public Shape shape;
        public Sprite icon;
    }

    public enum E_ItemType
    {
        MODULE,
        WEAPON
    }
}
