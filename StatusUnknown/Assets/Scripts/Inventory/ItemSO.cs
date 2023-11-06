namespace Inventory
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/ItemSO", fileName = "ItemSO", order = 0)]
    public class ItemSO : ScriptableObject, IShaped
    {
        [field: SerializeField]
        public Shape Shape { get; set; }
        public Sprite view;
    }
}
