namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class GridDataSO : ScriptableObject
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        [SerializedDictionary]
        public SerializedDictionary<Vector2Int, Item.Item> content;
    }
}
