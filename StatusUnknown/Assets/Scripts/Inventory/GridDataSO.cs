namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class GridDataSO : ScriptableObject, IShaped
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        [SerializedDictionary, ReadOnly]
        public SerializedDictionary<Vector2Int, Item> content = new SerializedDictionary<Vector2Int, Item>();
        
        [Button("Test add item")]
        public void AddItem(Item item)
        {
            this.content[item.gridPosition] = item;
        }
        
        [Button("Test remove item")]
        public void RemoveItem(Vector2Int position)
        {
            content.Remove(position);
        }
    }
}
