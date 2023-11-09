namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Core.Helpers;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using Core.SingletonsSO;
    using Core.UI;
    using UnityEngine;
    using UnityEngine.UIElements;
    public class GridView : MonoBehaviour
    {
        [SerializeField, Required, FoldoutGroup("References")]
        private GridDataSO gridDataSo;
        [SerializeField, Required, FoldoutGroup("References")]
        private UIDocument uiDocument;
        [SerializeField, Required, FoldoutGroup("References")]
        private string gridParentName;
        
        private Slot[] slots;
        private HashSet<ItemView> itemsView = new HashSet<ItemView>();
        
        private VisualElement GridRoot
        {
            get
            {
                return gridRoot ??= uiDocument.rootVisualElement.Q<VisualElement>(this.gridParentName);
            }
        }
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

        #region GRID_DISPLAY
        [Button("Display"), HideInEditorMode, BoxGroup("Actions")]
        public void DisplayGrid()
        {
            this.GridRoot.style.display = DisplayStyle.Flex;
            VisualElement firstFocus = this.BuildGrid();
            UIHandler.Instance.ForceFocus(firstFocus);
        }

        [Button("Hide"), HideInEditorMode, BoxGroup("Actions")]
        public void HideGrid()
        {
            this.GridRoot.style.display = DisplayStyle.None;
        }
        #endregion //GRID_DISPLAY

        #region GRID_BUILD
        private VisualElement BuildGrid()
        {
            VisualElement firstFocusElement = null;
            Shape gridShape = this.gridDataSo.Shape;
            List<Slot> slotsList = new List<Slot>();
            VisualTreeAsset slotTemplate = UIHandler.Instance.uiSettings.slotTreeAsset;
            VisualElement verticalParent = this.GridRoot.Q<VisualElement>("verticalParent");
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
                        slot.focusElement.RegisterCallback<FocusEvent>(e => this.GridElementFocusEvent(slot));
                        slot.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(slot));
                        firstFocusElement ??= gridSlotElement;
                    }
                    else
                    {
                        slotView.AddToClassList("hiddenSlot");
                    }
                }
            }

            this.slots = slotsList.ToArray();
            return firstFocusElement;
        }
        #endregion //GRID_BUILD

        #region CONTENT_SAVE_LOAD
        [Button("Load content"), HideInEditorMode, BoxGroup("Actions")]
        private void LoadContent()
        {
            ClearContent(false);
            foreach (KeyValuePair<Vector2Int, Item> info in gridDataSo.content)
            {
                ItemView itemView = new ItemView(info.Value, info.Key, this);
                this.SetItemPosition(itemView, info.Key);
                this.itemsView.Add(itemView);
                itemView.focusElement.RegisterCallback<FocusEvent>(e => this.GridElementFocusEvent.Invoke(itemView));
                itemView.focusElement.RegisterCallback<NavigationSubmitEvent>(e => this.OnInteract(itemView));
            }
        }
        
        [Button("Save content"), HideInEditorMode, BoxGroup("Actions")]
        private void SaveContent()
        {
            SerializedDictionary<Vector2Int, Item> newContent = new SerializedDictionary<Vector2Int, Item>();
            foreach (ItemView itemView in this.itemsView)
                newContent.Add(itemView.gridPosition, itemView.item);
            
            gridDataSo.content = newContent;
        }
        
        [Button("Clear content"), HideInEditorMode, BoxGroup("Actions")]
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
            this.GridRoot.Add(itemView.viewRoot);
            Vector3 newPosition = (Vector2)position * SlotWidth;
            itemView.viewRoot.transform.position = newPosition;
        }
        #endregion
        
        #region CONTENT_ACTIONS
        public void OnPickItem(ItemView itemView)
        {
            itemView.MoveState();
            SetSlotsContent(itemView.item.itemDefinition.Shape, itemView.gridPosition, null);
            UIHandler.Instance.ForceFocus(GetSlot(itemView.gridPosition).focusElement);
            this.itemsView.Remove(itemView);
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
            
            Debug.Log($"Set item to {pos}");
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
        
        #region UNITY_METHODS
        private void OnEnable()
        {
            this.GridElementFocusEvent += UIHandler.Instance.OnGridElementFocus;
        }

        private void OnDisable()
        {
            this.GridElementFocusEvent -= UIHandler.Instance.OnGridElementFocus;
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
