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
        
        private GridView selectedGrid;
        
        public void ForceFocus(VisualElement element)
        {
            element.Focus();
        }

        #region GRID_MANAGEMENT
        public void OnGridElementFocus(IGridElement element)
        {
            this.selectedGrid = element.Grid;
            if(this.isMovingItem)
                MoveItem(element);
        }

        public void TryPickItem(ItemView itemView)
        {
            if(this.isMovingItem)
                return;
            PickItem(itemView);
        }
        
        public void PickItem(ItemView itemView)
        {
            this.movingItem = itemView;
            this.isMovingItem = true;
            
            itemView.MoveState();
            itemView.Grid.OnPickItem(itemView);
        }

        private void MoveItem(IGridElement focusedElement)
        {
            this.movingItem.CanPlaceItemFeedback(this.selectedGrid.CanPlaceItem(this.movingItem.item.GridItemDefinition.shape, focusedElement.GridPosition));
            this.selectedGrid.SetItemVisualPosition(this.movingItem, focusedElement.GridPosition);
            //Debug.Log($"Move item to {focusedElement.gridPosition} in grid parent {this.selectedGrid}.");
        }

        public void TryDropItem(Vector2Int pos)
        {
            if (this.selectedGrid.CanPlaceItem(this.movingItem.item.GridItemDefinition.shape, pos))
                this.DropItem(pos);
        }

        private void DropItem(Vector2Int pos)
        {
            this.movingItem.PlacedState();
            this.selectedGrid.DropItem(this.movingItem, pos);
            this.ForceFocus(this.movingItem.FocusElement);
            
            this.movingItem = null;
            this.isMovingItem = false;
        }
        #endregion //GRID_MANAGEMENT
    }
}
