namespace Inventory.Grid
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using Core.SingletonsSO;
    using Containers;
    using Item;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class GridView
    {
        private IItemsDataContainer container;
        private Shape shape;
        private E_ItemType[] canContainsTypes;
        
        private Slot[] slots;
        private readonly HashSet<ItemView> itemsView = new HashSet<ItemView>();
        
        private readonly VisualElement gridRoot;

        private float SlotWidth
        {
            get
            {
                if (this.slotWidth == 0)
                    this.slotWidth = UIHandler.Instance.uiSettings.slotWidth;
                return this.slotWidth;
            }
        }
        private float slotWidth;

        private Action<IGridElement> gridElementFocusEvent;

        private VisualElement firstFocus;
        
        #region CONSTRUCTOR
        public GridView(VisualElement root, Shape shape, IItemsDataContainer container, E_ItemType[] typesContained)
        {
            this.gridRoot = root;
            this.shape = shape;
            this.container = container;
            this.gridElementFocusEvent += UIHandler.Instance.OnGridElementFocus;
            this.canContainsTypes = typesContained;
            this.BuildGrid();
        }
        #endregion //CONSTRUCTOR

        #region GRID_DISPLAY
        public void FocusOnGrid()
        {
            UIHandler.Instance.ForceFocus(this.firstFocus);
        }
        #endregion //GRID_DISPLAY

        #region GRID_BUILD
        private void BuildGrid()
        {
            this.firstFocus = null;
            VisualElement verticalParent = this.gridRoot.Q<VisualElement>("verticalParent");
            
            VisualElement[] gridSlots = GridBuilder.BuildGrid(this.shape, verticalParent, UIHandler.Instance.uiSettings.slotTemplate);

            List<Slot> slotsList = new List<Slot>();
            for (int y = 0; y < this.shape.shapeSize.y; y++)
            {
                for (int x = 0; x < this.shape.shapeSize.x; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    VisualElement slotView = gridSlots[GridHelper.GetIndexFromGridPosition(pos, this.shape.shapeSize.x)];
                    Slot slot = new Slot(pos, slotView, this);
                    slot.FocusElement.RegisterCallback<FocusEvent>(e => this.gridElementFocusEvent?.Invoke(slot));
                    slot.FocusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(slot));
                    slotsList.Add(slot);
                    if (!slotView.ClassListContains("hiddenSlot"))
                        this.firstFocus ??= slot.FocusElement;
                }
            }
            this.slots = slotsList.ToArray();
        }
        #endregion //GRID_BUILD
        
        #region CONTENT_SAVE_LOAD
        public void LoadNewData(Shape shape, IItemsDataContainer newContainer)
        {
            this.ClearContent(false);
            
            this.shape = shape;
            this.container = newContainer;
            this.BuildGrid();
            this.LoadContent();
        }
        
        public void LoadContent()
        {
            this.ClearContent(false);
            foreach (var info in this.container.GetAllItems())
            {
                if (info.Value == null)
                {
                    Debug.LogWarning($"Try to spawn null item at {info.Key}. Item skipped.");
                    continue;
                }
                ItemView itemView = ItemView.InstantiateItemView(info.Value, info.Key, this);
                this.SetItemPosition(itemView, info.Key);
                this.itemsView.Add(itemView);
                itemView.FocusElement.RegisterCallback<FocusEvent>(e => this.gridElementFocusEvent?.Invoke(itemView));
                itemView.FocusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(itemView));
            }
        }
        
        private void SaveContent()
        {
            VectorIntItemDictionary dictionary = new VectorIntItemDictionary();
            foreach (ItemView itemView in this.itemsView)
                dictionary.Add(itemView.GridPosition, itemView.ItemData);
            
            this.container.SaveAllItems(dictionary);
        }
        
        private void ClearContent(bool clearData)
        {
            HashSet<ItemView> tempItem = new HashSet<ItemView>(this.itemsView);
            foreach (ItemView itemView in tempItem)
                this.RemoveItem(itemView);
            
            this.itemsView.Clear();

            if (clearData)
                this.container.ClearData();
        }
        #endregion //CONTENT_SAVE_LOAD

        #region CONTENT_MANAGEMENT
        private void SetSlotsContent(Shape shape, Vector2Int pos, ItemView content)
        {
            Slot[] positions = this.GetSlotsFromShape(shape, pos);
            
            foreach (var slot in positions)
                slot.SetOccupied(content);
        }
        
        public void SetItemVisualPosition(ItemView itemView, Vector2Int position)
        {
            this.gridRoot.Add(itemView.ViewRoot);
            Vector3 newPosition = (Vector2)position * this.SlotWidth;
            itemView.ViewRoot.transform.position = newPosition;
        }
        #endregion
        
        #region CONTENT_ACTIONS
        public void OnPickItem(ItemView itemView)
        {
            this.SetSlotsContent(itemView.ItemData.GridItemDefinition.shape, itemView.GridPosition, null);
            this.itemsView.Remove(itemView);
            UIHandler.Instance.ForceFocus(this.GetSlot(itemView.GridPosition).FocusElement);
            this.SaveContent();
        }
        
        public void DropItem(ItemView itemView, Vector2Int pos)
        {
            this.SetItemPosition(itemView, pos);
            this.SaveContent();
        }
        
        private void SetItemPosition(ItemView itemView, Vector2Int pos)
        {
            //Set visual
            this.SetItemVisualPosition(itemView, pos);

            itemView.Grid = this;
            itemView.GridPosition = pos;
            
            //Setup slots infos under the item
            this.SetSlotsContent(itemView.ItemData.GridItemDefinition.shape, pos, itemView);
            
            //Register item in grid
            this.itemsView.Add(itemView);
        }
        
        private void RemoveItem(ItemView itemView)
        {
            itemView.ViewRoot.RemoveFromHierarchy();
            this.itemsView.Remove(itemView);

            this.SetSlotsContent(itemView.ItemData.GridItemDefinition.shape, itemView.GridPosition, null);
        }
        #endregion //CONTENT_ACTIONS
        
        #region EVENTS
        private void OnInteract(IGridElement element)
        {
            //If we interact with an item, we pick it
            if (element is ItemView itemView)
            {
                UIHandler.Instance.TryPickItem(itemView);
            }
            else if (element is Slot slot) //If we interact with a slot and we're moving an item, we try to drop it
            {
                if (UIHandler.Instance.isMovingItem)
                    UIHandler.Instance.TryDropItem(slot.GridPosition);
            }
        }
        #endregion

        #region UTILITIES
        public bool CanPlaceItem(ItemView itemView, Vector2Int pos)
        {
            Shape itemShape = itemView.ItemData.GridItemDefinition.shape;
            Vector2Int[] itemShapeCoord = itemShape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                Vector2Int currentPosition = coord + pos;
                if (!this.shape.GetContentFromPosition(currentPosition) || !GridHelper.IsInGrid(currentPosition, this.shape.shapeSize) || this.GetSlot(currentPosition).item != null || !CanContainsItem(itemView))
                {
                    //Debug.LogWarning($"try to setup slot state at {coord + pos} but the position is invalid.");
                    return false;
                }
            }
            return true;
        }

        public bool CanContainsItem(ItemView itemView)
        {
            foreach (var itemType in this.canContainsTypes)
            {
                if (itemType == itemView.ItemData.GridItemDefinition.ItemType)
                    return true;
            }

            return false;
        }

        private Slot[] GetSlotsFromShape(Shape shape, Vector2Int pos)
        {
            List<Slot> result = new List<Slot>();
            Vector2Int[] itemShapeCoord = shape.GetPositionsRelativeToAnchor();
            
            foreach (var coord in itemShapeCoord)
            {
                if (!this.shape.GetContentFromPosition(coord + pos))
                {
                    Debug.LogWarning($"try to get slot from {coord + pos} but this slot doesn't exists.");
                    continue;
                }
                result.Add(this.GetSlot(coord + pos));
            }

            return result.ToArray();
        }

        private Slot GetSlot(Vector2Int pos)
        {
            if (!this.shape.GetContentFromPosition(pos))
            {
                Debug.LogWarning($"try to setup slot state at {pos}. but this slot doesn't exists.");
                return null;
            }
            return this.slots[GridHelper.GetIndexFromGridPosition(pos, this.shape.shapeSize.x)];
        }
        #endregion //UTILITIES
    }
}
