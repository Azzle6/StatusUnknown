namespace Core.SingletonsSO
{
    using Inventory;
    using Inventory.Item;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/PlayerHandler", fileName = "PlayerHandler")]
    public class PlayerHandler : SingletonSO<PlayerHandler>
    {
        public PlayerInventorySO playerInventory;

        [Button]
        public void AddItemToInventory(ItemData item)
        {
            Debug.Log("Add item to inventory.");
            this.playerInventory.InventoryData.AddNewItem(item);
        }
    }
}
