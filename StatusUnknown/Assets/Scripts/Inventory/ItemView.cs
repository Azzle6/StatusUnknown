namespace Inventory
{
    using System;
    using Core.Helpers;
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.UIElements;

    [Serializable]
    public class ItemView : GridElement
    {
        public Item item;

        public void GenerateView()
        {
            Vector2Int shapeSize = item.itemDefinition.Shape.shapeSize;
            VisualElement itemView = UIInputsHandler.Instance.uiSettings.itemTemplate.Instantiate();
            itemView.style.position = Position.Absolute;
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

        public ItemView(Item item, Vector2Int gridPosition, GridView gridView)
        {
            this.item = item;
            this.gridPosition = gridPosition;
            this.grid = gridView;
        }
    }
}
