namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Core.Helpers;
    using System;
    using System.Collections.Generic;
    using Core.SingletonsSO;
    using Item;
    using UnityEngine;
    using UnityEngine.UIElements;
    public class GridView
    {
        private SerializedDictionary<Vector2Int, Item.Item> content;
        private Shape shape;
        
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

        private Action<GridElement> gridElementFocusEvent;

        private VisualElement firstFocus;
        
        #region CONSTRUCTOR
        public GridView(VisualElement root, Shape shape, SerializedDictionary<Vector2Int, Item.Item> content)
        {
            this.gridRoot = root;
            this.shape = shape;
            this.content = content;
            this.BuildGrid();
        }
        #endregion //CONSTRUCTOR

        #region GRID_DISPLAY
        public void FocusOnGrid()
        {
            UIHandler.Instance.ForceFocus(firstFocus);
        }

        public void HideGrid()
        {
            this.gridRoot.style.display = DisplayStyle.None;
        }
        #endregion //GRID_DISPLAY

        #region GRID_BUILD
        private void BuildGrid()
        {
            this.firstFocus = null;
            Shape gridShape = this.shape;
            List<Slot> slotsList = new List<Slot>();
            VisualTreeAsset slotTemplate = UIHandler.Instance.uiSettings.slotTreeAsset;
            VisualElement verticalParent = this.gridRoot.Q<VisualElement>("verticalParent");
            verticalParent.Clear();

            for (int y = 0; y < gridShape.shapeSize.y; y++)
            {
                VisualElement horizontalParent = new VisualElement();
                horizontalParent.AddToClassList("horizontalParent");
                verticalParent.Insert(y, horizontalParent);
                
                for (int x = 0; x < gridShape.shapeSize.x; x++)
                {
                    VisualElement slotView = slotTemplate.Instantiate();
                    
                    horizontalParent.Insert(x, slotView);
                    VisualElement gridSlotElement = slotView.Q<VisualElement>("gridSlot");
                    Slot slot = new Slot(new Vector2Int(x, y), slotView, this);
                    slotsList.Add(slot);

                    if (this.shape.GetContentFromPosition(new Vector2Int(x, y)))
                    {
                        gridSlotElement.name = $"{x},{y}";
                        slot.focusElement.RegisterCallback<FocusEvent>(e => this.gridElementFocusEvent?.Invoke(slot));
                        slot.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(slot));
                        this.firstFocus ??= gridSlotElement;
                    }
                    else
                    {
                        slotView.AddToClassList("hiddenSlot");
                    }
                }
            }
            
            this.gridElementFocusEvent += UIHandler.Instance.OnGridElementFocus;

            this.slots = slotsList.ToArray();
        }
        #endregion //GRID_BUILD
        
        #region CONTENT_SAVE_LOAD
        public void LoadNewData(Shape shape, SerializedDictionary<Vector2Int, Item.Item> content)
        {
            ClearContent(false);
            
            this.shape = shape;
            this.content = content;
            this.BuildGrid();
            this.LoadContent();
        }
        
        public void LoadContent()
        {
            ClearContent(false);
            foreach (KeyValuePair<Vector2Int, Item.Item> info in this.content)
            {
                ItemView itemView = new ItemView(info.Value, info.Key, this);
                this.SetItemPosition(itemView, info.Key);
                this.itemsView.Add(itemView);
                itemView.focusElement.RegisterCallback<FocusEvent>(e => this.gridElementFocusEvent?.Invoke(itemView));
                itemView.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(itemView));
            }
        }
        
        private void SaveContent()
        {
            this.content.Clear();
            foreach (ItemView itemView in this.itemsView)
                this.content.Add(itemView.gridPosition, itemView.item);
        }
        
        private void ClearContent(bool clearData)
        {
            HashSet<ItemView> tempItem = new HashSet<ItemView>(this.itemsView);
            foreach (ItemView itemView in tempItem)
                this.RemoveItem(itemView);
            
            this.itemsView.Clear();

            if (clearData)
                this.content.Clear();
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
            this.gridRoot.Add(itemView.viewRoot);
            Vector3 newPosition = (Vector2)position * SlotWidth;
            itemView.viewRoot.transform.position = newPosition;
        }
        #endregion
        
        #region CONTENT_ACTIONS
        public void OnPickItem(ItemView itemView)
        {
            SetSlotsContent(itemView.item.itemDefinition.Shape, itemView.gridPosition, null);
            this.itemsView.Remove(itemView);
            UIHandler.Instance.ForceFocus(GetSlot(itemView.gridPosition).focusElement);
            this.SaveContent();
        }
        
        public void DropItem(ItemView itemView, Vector2Int pos)
        {
            SetItemPosition(itemView, pos);
            this.SaveContent();
        }
        
        private void SetItemPosition(ItemView itemView, Vector2Int pos)
        {
            //Set visual
            SetItemVisualPosition(itemView, pos);

            itemView.grid = this;
            itemView.gridPosition = pos;
            
            //Setup slots infos under the item
            SetSlotsContent(itemView.item.itemDefinition.Shape, pos, itemView);
            
            //Register item in grid
            this.itemsView.Add(itemView);
        }
        
        private void RemoveItem(ItemView itemView)
        {
            itemView.viewRoot.RemoveFromHierarchy();
            this.itemsView.Remove(itemView);

            this.SetSlotsContent(itemView.item.itemDefinition.Shape, itemView.gridPosition, null);
        }
        #endregion //CONTENT_ACTIONS
        
        #region EVENTS
        private void OnInteract(GridElement element)
        {
            //If we interact with an item, we pick it
            if (element is ItemView itemView)
            {
                UIHandler.Instance.PickItem(itemView);
            }
            else if (element is Slot slot) //If we interact with a slot and we're moving an item, we try to drop it
            {
                if (UIHandler.Instance.isMovingItem)
                    UIHandler.Instance.TryDropItem(slot.gridPosition);
            }
        }
        #endregion

        #region UTILITIES
        public bool CanPlaceItem(Shape itemShape, Vector2Int pos)
        {
            Vector2Int[] itemShapeCoord = itemShape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                Vector2Int currentPosition = coord + pos;
                if (!this.shape.GetContentFromPosition(currentPosition) || !GridHelper.IsInGrid(currentPosition, this.shape.shapeSize))
                {
                    Debug.LogWarning($"try to setup slot state at {coord + pos} but the position is invalid.");
                    return false;
                }
            }

            return true;
        }

        private Slot[] GetSlotsFromShape(Shape shape, Vector2Int pos)
        {
            List<Slot> result = new List<Slot>();
            Vector2Int[] itemShapeCoord = shape.GetPositionsRelativeToAnchor();
            
            foreach (var coord in itemShapeCoord)
            {
                if (!this.shape.GetContentFromPosition(coord + pos))
                {
                    Debug.LogWarning($"try to setup slot state at {coord + pos}. but this slot doesn't exists.");
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
