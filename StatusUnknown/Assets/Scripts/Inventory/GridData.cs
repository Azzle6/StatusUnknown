namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using UnityEngine;

    public class GridData<T> where T : Item
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        [SerializedDictionary]
        public SerializedDictionary<Vector2Int, T> content = new SerializedDictionary<Vector2Int, T>();
    }
}
