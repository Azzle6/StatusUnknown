namespace Inventory.Item
{
    using System;
    using Core.Helpers;
    using Core.SingletonsSO;
    using Core.UI;
    using Grid;
    using Module;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Weapons;

    [Serializable]
    public abstract class ItemView : IGridElement
    {
        public GridView Grid { get; set; }
        public Vector2Int GridPosition { get; set; }
        public VisualElement ViewRoot { get; set; }
        public VisualElement FocusElement { get; set; }
        
        public ItemData ItemData;
        
        private VisualElement verticalRoot;
        private VisualElement iconElement;
        protected UISettings UiSettings;

        #region CONSTRUCTOR
        protected ItemView(ItemData itemData, Vector2Int gridPosition, GridView gridView)
        {
            this.UiSettings = UIHandler.Instance.uiSettings;
            this.ItemData = itemData;
            this.GridPosition = gridPosition;
            this.Grid = gridView;
            this.GenerateBaseView();
        }
        #endregion
        
        private void GenerateBaseView()
        {
            if (this.iconElement != null)
            {
                this.iconElement.RemoveFromHierarchy();
                this.iconElement = null;
            }
            
            Shape shape = this.ItemData.GridItemDefinition.shape;
            VisualElement itemView = this.UiSettings.itemTemplate.Instantiate();
            itemView.style.position = Position.Absolute;
            
            this.verticalRoot = itemView.Q<VisualElement>("verticalRoot");
            GridBuilder.BuildGrid(shape, this.verticalRoot, this.UiSettings.itemSquareTemplate);

            
            
            for (int y = 0; y < shape.shapeSize.y; y++)
            {
                for (int x = 0; x < shape.shapeSize.x; x++)
                {
                    Vector2Int currentPosition = new Vector2Int(x, y);
                    if (shape.IsValidPosition(currentPosition))
                    {
                        if (this.iconElement == null)
                        {
                            this.iconElement = UiSettings.itemIconTemplate.Instantiate();
                            this.iconElement.Q<VisualElement>("itemIcon").style.backgroundImage = this.ItemData.GridItemDefinition.icon.texture;
                            itemView.Add(this.iconElement);
                            this.iconElement.transform.position = (Vector2)currentPosition * this.UiSettings.slotSize;
                        }
                        foreach (var direction in Enum.GetValues(typeof(E_Direction)))
                        {
                            if(shape.IsValidPosition(currentPosition + GridHelper.DirectionToVectorInt((E_Direction)direction, true)))
                               HideEdge(currentPosition, (E_Direction)direction);
                        }
                    }
                }
            }

            void HideEdge(Vector2Int pos, E_Direction direction)
            {
                VisualElement edgeHide = this.UiSettings.itemEdgesHideTemplate.Instantiate();
                float height = 20;

                Vector2 directionDisplacement;
                float rotation = 0;
                float slotSize = this.UiSettings.slotSize;
                
                switch (direction)
                {
                    case E_Direction.UP:
                        directionDisplacement = new Vector2(0, -height/2);
                        break;
                    case E_Direction.DOWN:
                        directionDisplacement = new Vector2(0, slotSize - height/2);
                        break;
                    case E_Direction.LEFT:
                        directionDisplacement = new Vector2(height/2, 0);
                        rotation = 90;
                        break;
                    case E_Direction.RIGHT:
                        directionDisplacement = new Vector2(slotSize + height/2f , 0);
                        rotation = 90;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                itemView.Add(edgeHide);
                edgeHide.transform.position = (Vector2)pos * this.UiSettings.slotSize + directionDisplacement;
                edgeHide.style.rotate = new StyleRotate(new Rotate(rotation));
            }
            
            this.iconElement.BringToFront();
            if (this.ViewRoot != null)
                this.ViewRoot.RemoveFromHierarchy();
            this.ViewRoot = itemView;
            this.FocusElement = itemView.Q<VisualElement>("focusParent");
        }

        protected abstract void GenerateCustomView();

        public void SetMoveState()
        {
            this.FocusElement.AddToClassList("movingElement");
        }

        public void SetPlaceItemFeedback(bool canPlace)
        {
            this.FocusElement.AddToClassList(canPlace ? "canPlace" : "cantPlace");
            this.FocusElement.RemoveFromClassList(canPlace ? "cantPlace" : "canPlace");
        }

        public void PlacedState()
        {
            this.FocusElement.RemoveFromClassList("movingElement");
            this.FocusElement.RemoveFromClassList("canPlace");
            this.FocusElement.RemoveFromClassList("cantPlace");
        }

        public static ItemView InstantiateItemView(ItemData itemData, Vector2Int gridPosition, GridView gridView)
        {
            switch (itemData.GridItemDefinition.ItemType)
            {
                case E_ItemType.MODULE:
                    return new ModuleItemView((ModuleData)itemData, gridPosition, gridView);
                case E_ItemType.WEAPON:
                    return new WeaponItemView((WeaponData)itemData, gridPosition, gridView);
            }

            return null;
        }
    }
}
