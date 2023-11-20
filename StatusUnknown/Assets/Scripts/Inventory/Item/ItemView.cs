namespace Inventory.Item
{
    using System;
    using Core.Helpers;
    using Core.SingletonsSO;
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
        
        public ItemData DataDataItemData;
        private VisualElement verticalRoot;

        #region CONSTRUCTOR
        protected ItemView(ItemData dataDataItemData, Vector2Int gridPosition, GridView gridView)
        {
            this.DataDataItemData = dataDataItemData;
            this.GridPosition = gridPosition;
            this.Grid = gridView;
            this.GenerateBaseView();
            this.GenerateCustomView();
        }
        #endregion
        
        public void GenerateBaseView()
        {
            if (this.ViewRoot != null)
                this.ViewRoot.parent.Remove(this.ViewRoot);
            
            Shape shape = this.DataDataItemData.GridItemDefinition.shape;
            VisualElement itemView = UIHandler.Instance.uiSettings.itemTemplate.Instantiate();
            itemView.style.position = Position.Absolute;
            
            this.verticalRoot = itemView.Q<VisualElement>("verticalRoot");
            VisualElement[] slots = GridBuilder.BuildGrid(shape, this.verticalRoot, UIHandler.Instance.uiSettings.itemSquareTemplate);
            
            this.ViewRoot = itemView;
            this.FocusElement = itemView.Q<VisualElement>("focusParent");
        }

        protected abstract void GenerateCustomView();

        public void MoveState()
        {
            this.FocusElement.AddToClassList("movingElement");
            this.FocusElement.focusable = false;
        }

        public void CanPlaceItemFeedback(bool canPlace)
        {
            this.FocusElement.AddToClassList(canPlace ? "canPlace" : "cantPlace");
            this.FocusElement.RemoveFromClassList(canPlace ? "cantPlace" : "canPlace");
        }

        public void PlacedState()
        {
            this.FocusElement.RemoveFromClassList("movingElement");
            this.FocusElement.RemoveFromClassList("canPlace");
            this.FocusElement.RemoveFromClassList("cantPlace");
            this.FocusElement.focusable = true;
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
