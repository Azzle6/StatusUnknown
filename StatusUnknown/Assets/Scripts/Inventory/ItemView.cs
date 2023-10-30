namespace Inventory
{
    using Core.Helpers;
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ItemView
    {
        public Item item;
        public VisualElement view;
        public Vector2Int gridPosition;

        public void GenerateView()
        {
            Vector2Int shapeSize = item.itemDefinition.Shape.shapeSize;
            VisualElement itemView = UISettings.Instance.itemTemplate.Instantiate();
            VisualElement verticalRoot = itemView.Q<VisualElement>("verticalRoot");
            for (int y = 0; y < shapeSize.y; y++)
            {
                VisualElement horizontalParent = new VisualElement();
                horizontalParent.AddToClassList("horizontalParent");
                verticalRoot.Insert(y, horizontalParent);
                
                for (int x = 0; x < shapeSize.x; x++)
                {
                    VisualElement slot = new VisualElement();
                    
                    horizontalParent.Insert(x, slot);

                    slot.AddToClassList(this.item.itemDefinition.Shape.shapeContent[
                        GridHelper.GetIndexFromGridPosition(new Vector2Int(x, y), shapeSize.x)]
                        ? "baseSlot"
                        : "hiddenSlot");
                }
            }
            this.view = itemView;
        }

        public ItemView(Item item)
        {
            this.item = item;
        }
    }
}
