namespace Inventory
{
    using UnityEngine;
    using Weapons;

    [CreateAssetMenu(menuName = "CustomAssets/Data/PlayerInventory")]
    public class PlayerInventorySO : ScriptableObject
    {
        public GridDataSO inventory;
        public Weapon[] equippedWeaponsData;
    }
}