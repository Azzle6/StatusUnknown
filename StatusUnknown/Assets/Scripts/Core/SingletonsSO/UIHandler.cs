namespace Core.SingletonsSO
{
    using Inventory.Grid;
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
        private ItemView movingItem;
        
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
        
        private void PickItem(ItemView itemView)
        {
            this.movingItem = itemView;
            this.isMovingItem = true;
            
            itemView.MoveState();
            itemView.Grid.OnPickItem(itemView);
        }

        private void MoveItem(IGridElement focusedElement)
        {
            bool canPlace =
                this.selectedGrid.CanPlaceItem(this.movingItem, focusedElement.GridPosition);
            
            this.movingItem.CanPlaceItemFeedback(canPlace);
            this.selectedGrid.SetItemVisualPosition(this.movingItem, focusedElement.GridPosition);
            //Debug.Log($"Move item to {focusedElement.gridPosition} in grid parent {this.selectedGrid}.");
        }

        public void TryDropItem(Vector2Int pos)
        {
            if (this.selectedGrid.CanPlaceItem(this.movingItem, pos))
                this.DropItem(this.selectedGrid, pos);
            else
                this.CancelItemMoving();
                
        }

        private void DropItem(GridView grid, Vector2Int pos)
        {
            this.movingItem.PlacedState();
            grid.DropItem(this.movingItem, pos);
            this.ForceFocus(this.movingItem.FocusElement);
            
            this.movingItem = null;
            this.isMovingItem = false;
        }

        private void CancelItemMoving()
        {
            if(this.isMovingItem)
                DropItem(this.movingItem.Grid, this.movingItem.GridPosition);
        }
        #endregion //GRID_MANAGEMENT
    }
}
