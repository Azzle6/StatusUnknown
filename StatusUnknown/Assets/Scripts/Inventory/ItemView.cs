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
        private VisualElement verticalRoot;

        #region CONSTRUCTOR
        public ItemView(Item item, Vector2Int gridPosition, GridView gridView)
        {
            this.item = item;
            this.gridPosition = gridPosition;
            this.grid = gridView;
            this.GenerateView();
        }
        #endregion
        
        public void GenerateView()
        {
            if (this.viewRoot != null)
                this.viewRoot.parent.Remove(this.viewRoot);
            
            Vector2Int shapeSize = item.itemDefinition.Shape.shapeSize;
            VisualElement itemView = UIHandler.Instance.uiSettings.itemTemplate.Instantiate();
            itemView.style.position = Position.Absolute;
            verticalRoot = itemView.Q<VisualElement>("verticalRoot");
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
            this.viewRoot = itemView;
            this.focusElement = itemView.Q<VisualElement>("focusParent");
        }

        public void MoveState()
        {
            this.verticalRoot.AddToClassList("movingElement");
            this.focusElement.focusable = false;
        }

        public void PlacedState()
        {
            this.verticalRoot.RemoveFromClassList("movingElement");
            this.focusElement.focusable = true;
        }
    }
}
