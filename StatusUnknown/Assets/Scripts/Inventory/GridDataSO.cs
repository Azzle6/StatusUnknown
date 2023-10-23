namespace Inventory
{
    using UnityEngine;
    using Core.Helpers;
    using Sirenix.OdinInspector;

    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class GridDataSO : ScriptableObject, IShaped
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        [SerializeField]
        public SerializableDictionary<Vector2Int, ItemSO> content = new SerializableDictionary<Vector2Int, ItemSO>();
        
        public void AddItem(ItemSO item, Vector2Int position)
        {
            if (content.ContainsKey(position))
                content[position] = item;
            else
                content.Add(position, item);
        }
        public void RemoveItem(Vector2Int position)
        {
            content.Remove(position);
        }
    }
}
