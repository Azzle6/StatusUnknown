namespace Inventory
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Data", fileName = "ItemSO", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public Shape shape;
        public Sprite view;
    }
}
