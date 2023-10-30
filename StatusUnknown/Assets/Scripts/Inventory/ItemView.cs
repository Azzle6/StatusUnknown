namespace Inventory
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ItemView
    {
        public Item item;
        public VisualElement view;
        public Vector2Int gridPosition;

        public void GenerateView()
        {
            
        }

        public ItemView(Item item)
        {
            this.item = item;
        }
    }
}
