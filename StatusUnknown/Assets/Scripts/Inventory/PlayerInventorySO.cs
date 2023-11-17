namespace Inventory
{
    using UnityEngine;
    using Weapons;

    [CreateAssetMenu(menuName = "CustomAssets/Data/PlayerInventory")]
    public class PlayerInventorySO : ScriptableObject
    {
        public InventoryDataSO inventory;
        public Weapon[] equippedWeaponsData;
    }
}