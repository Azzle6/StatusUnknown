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
        [SerializeField, Required]
        private GridDataSO gridDataSo;
        [SerializeField, Required]
        private UIDocument uiDocument;
        [SerializeField, Required]
        private string gridParentName;
        
        [SerializeField]
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

        #region GRID_DISPLAY
        [Button("Display"), HideInEditorMode, BoxGroup("Actions")]
        public void DisplayGrid()
        {
            this.GridRoot.style.display = DisplayStyle.Flex;
            VisualElement firstFocus = this.BuildGrid();
            UIHandler.Instance.SetFocus(firstFocus);
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
                    Slot slot = new Slot(new Vector2Int(x, y), gridSlotElement, this);
                    slotsList.Add(slot);

                    if (this.gridDataSo.Shape.GetContentFromPosition(new Vector2Int(x, y)))
                    {
                        gridSlotElement.name = $"{x},{y}";
                        this.RegisterElementEvents(slot);
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

        private void RegisterElementEvents(GridElement gridElement)
        {
            gridElement.view.RegisterCallback<FocusEvent>(e => UIHandler.Instance.OnElementFocus(gridElement));
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
                itemView.GenerateView();
                this.RefreshItemPosition(itemView, info.Key);
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
        #endregion

        #region CONTENT_MANAGEMENT
        private void RefreshItemPosition(ItemView itemView, Vector2Int position)
        {
            //Set item visual position.
            this.GridRoot.Add(itemView.view);
            Vector3 newPosition = (Vector2)position * SlotWidth;
            itemView.view.transform.position = newPosition;
            
            itemView.gridPosition = position;
            this.itemsView.Add(itemView);
            
            SetSlotsOccupied(itemView.item.itemDefinition.Shape, position, true);
            
            //Setup item events
            this.RegisterElementEvents(itemView);
        }

        private void SetSlotsOccupied(Shape itemShape, Vector2Int pos, bool isOccupied)
        {
            Vector2Int[] itemShapeCoord = itemShape.GetPositionsRelativeToAnchor();
            
            foreach (var coord in itemShapeCoord)
            {
                if (!gridDataSo.Shape.GetContentFromPosition(coord + pos))
                    Debug.LogWarning($"try to setup slot state at {coord + pos}. but this slot doesn't exists.");
                
                int index = GridHelper.GetIndexFromGridPosition(coord + pos, gridDataSo.Shape.shapeSize.x);
                this.slots[index].SetOccupied(isOccupied);
            }
        }
        
        private void RemoveItem(ItemView itemView)
        {
            itemView.view.RemoveFromHierarchy();
            this.itemsView.Remove(itemView);

            this.SetSlotsOccupied(itemView.item.itemDefinition.Shape, itemView.gridPosition, false);
        }
        #endregion
    }
}
