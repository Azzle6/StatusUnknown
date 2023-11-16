namespace Inventory
{
    using Core.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Data/GridData", fileName = "GridData", order = 0)]
    public class GridDataSO : ScriptableObject
    {
        [field: SerializeField]
        public Shape Shape { get; set; }

        public VectorIntItemDictionary content;
    }
}
