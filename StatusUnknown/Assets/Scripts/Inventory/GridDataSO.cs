namespace Inventory
{
    using UnityEngine;
    using Core.Helpers;
    
    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class GridDataSO : ScriptableObject
    {
        public Shape shape;
        public SerializableDictionary<Vector2Int, IGridItem> content = new SerializableDictionary<Vector2Int, IGridItem>();
    }
}
