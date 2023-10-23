namespace Inventory
{
    using UnityEngine;
    public class Item : MonoBehaviour
    {
        public ItemSO itemDefinition;
        public Vector2Int gridPosition;

        public Item(ItemSO itemDefinition, Vector2Int gridPosition)
        {
            this.itemDefinition = itemDefinition;
            this.gridPosition = gridPosition;
        }
    }
}
