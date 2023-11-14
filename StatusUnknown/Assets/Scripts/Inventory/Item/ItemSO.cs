namespace Inventory.Item
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/ItemSO", fileName = "ItemSO", order = 0)]
    public class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public Shape Shape { get; set; }
        public Sprite view;
    }
}
