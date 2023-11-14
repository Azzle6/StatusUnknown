namespace Core.SingletonsSO
{
    using System;
    using Inventory;
    using Inventory.Item;
    using Sirenix.OdinInspector;
    using UI;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/UIHandler", fileName = "UIHandler")]
    public class UIHandler : SingletonSO<UIHandler>
    {
        public UISettings uiSettings;
        
        [FoldoutGroup("Dynamic data")]
        public bool isMovingItem;
        [FoldoutGroup("Dynamic data")]
        public ItemView movingItem;

        public Action<GridElement> OnGridElementFocusEvent;
        
        private GridView selectedGrid;
        
        public void ForceFocus(VisualElement element)
        {
            element.Focus();
        }

        #region GRID_MANAGEMENT
        public void OnGridElementFocus(GridElement element)
        {
            this.selectedGrid = element.grid;
            this.OnGridElementFocusEvent?.Invoke(element);
        }

        public void PickItem(ItemView itemView)
        {
            this.movingItem = itemView;
            this.isMovingItem = true;
            
            itemView.MoveState();
            itemView.grid.OnPickItem(itemView);
            
            this.OnGridElementFocusEvent += this.MoveItem;
        }

        private void MoveItem(GridElement focusedElement)
        {
            this.movingItem.CanPlaceItemFeedback(this.selectedGrid.CanPlaceItem(this.movingItem.item.itemDefinition.Shape, focusedElement.gridPosition));
            this.selectedGrid.SetItemVisualPosition(this.movingItem, focusedElement.gridPosition);
            //Debug.Log($"Move item to {focusedElement.gridPosition} in grid parent {this.selectedGrid}.");
        }

        public void TryDropItem(Vector2Int pos)
        {
            if (this.selectedGrid.CanPlaceItem(this.movingItem.item.itemDefinition.Shape, pos))
                this.DropItem(pos);
        }

        private void DropItem(Vector2Int pos)
        {
            this.OnGridElementFocusEvent -= this.MoveItem;
            this.movingItem.PlacedState();
            this.selectedGrid.DropItem(this.movingItem, pos);
            this.ForceFocus(this.movingItem.focusElement);
            
            this.movingItem = null;
            this.isMovingItem = false;
        }
        #endregion //GRID_MANAGEMENT
    }
}
