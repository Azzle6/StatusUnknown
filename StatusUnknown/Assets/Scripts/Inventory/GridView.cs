namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Core.Helpers;
    using System;
    using System.Collections.Generic;
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.UIElements;
    public class GridView
    {
        private GridDataSO gridDataSo;
        
        private Slot[] slots;
        private HashSet<ItemView> itemsView = new HashSet<ItemView>();
        
        private VisualElement gridRoot;

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

        private Action<GridElement> GridElementFocusEvent;

        private VisualElement firstFocus;
        
        #region CONSTRUCTOR
        public GridView(VisualElement root, GridDataSO data)
        {
            this.gridRoot = root;
            this.gridDataSo = data;
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
            Shape gridShape = this.gridDataSo.Shape;
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

                    if (this.gridDataSo.Shape.GetContentFromPosition(new Vector2Int(x, y)))
                    {
                        gridSlotElement.name = $"{x},{y}";
                        slot.focusElement.RegisterCallback<FocusEvent>(e => this.GridElementFocusEvent?.Invoke(slot));
                        slot.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(slot));
                        this.firstFocus ??= gridSlotElement;
                    }
                    else
                    {
                        slotView.AddToClassList("hiddenSlot");
                    }
                }
            }
            
            this.GridElementFocusEvent += UIHandler.Instance.OnGridElementFocus;

            this.slots = slotsList.ToArray();
        }
        #endregion //GRID_BUILD

        #region CONTENT_SAVE_LOAD
        public void LoadNewContent(GridDataSO data)
        {
            ClearContent(false);
            
            this.gridDataSo = data;
            this.BuildGrid();
            this.LoadContent();
        }
        
        public void LoadContent()
        {
            ClearContent(false);
            foreach (KeyValuePair<Vector2Int, Item> info in gridDataSo.content)
            {
                ItemView itemView = new ItemView(info.Value, info.Key, this);
                this.SetItemPosition(itemView, info.Key);
                this.itemsView.Add(itemView);
                itemView.focusElement.RegisterCallback<FocusEvent>(e => this.GridElementFocusEvent?.Invoke(itemView));
                itemView.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(itemView));
            }
        }
        
        private void SaveContent()
        {
            SerializedDictionary<Vector2Int, Item> newContent = new SerializedDictionary<Vector2Int, Item>();
            foreach (ItemView itemView in this.itemsView)
                newContent.Add(itemView.gridPosition, itemView.item);
            
            gridDataSo.content = newContent;
        }
        
        private void ClearContent(bool clearData)
        {
            HashSet<ItemView> tempItem = new HashSet<ItemView>(this.itemsView);
            foreach (ItemView itemView in tempItem)
                this.RemoveItem(itemView);
            
            this.itemsView.Clear();

            if (clearData)
                gridDataSo.content.Clear();
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
        public bool CanPlaceItem(Shape shape, Vector2Int pos)
        {
            Vector2Int[] itemShapeCoord = shape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                if (!gridDataSo.Shape.GetContentFromPosition(coord + pos))
                {
                    Debug.LogWarning($"try to setup slot state at {coord + pos}. but this slot doesn't exists.");
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
                if (!gridDataSo.Shape.GetContentFromPosition(coord + pos))
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
            if (!gridDataSo.Shape.GetContentFromPosition(pos))
            {
                Debug.LogWarning($"try to setup slot state at {pos}. but this slot doesn't exists.");
                return null;
            }
            return this.slots[GridHelper.GetIndexFromGridPosition(pos, gridDataSo.Shape.shapeSize.x)];
        }
        #endregion //UTILITIES
    }
}
