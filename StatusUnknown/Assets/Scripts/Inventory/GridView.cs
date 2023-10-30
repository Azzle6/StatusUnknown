namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Core.Helpers;
    using Core.UI;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.SingletonsSO;
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

        #region GRID_DISPLAY
        [Button("Display"), HideInEditorMode, BoxGroup("Actions")]
        public void DisplayGrid()
        {
            this.GridRoot.style.display = DisplayStyle.Flex;
            VisualElement firstFocus = this.BuildGrid();
            UIManager.Instance.inputsHandler.SetFocus(firstFocus);
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
            VisualTreeAsset slotTemplate = UISettings.Instance.slotTreeAsset;
            VisualElement verticalParent = this.GridRoot.Q<VisualElement>("verticalParent");
            verticalParent.Clear();

            for (int y = 0; y < gridShape.shapeSize.y; y++)
            {
                VisualElement horizontalParent = new VisualElement();
                horizontalParent.AddToClassList("horizontalParent");
                verticalParent.Insert(y, horizontalParent);
                
                for (int x = 0; x < gridShape.shapeSize.x; x++)
                {
                    VisualElement slot = slotTemplate.Instantiate();
                    
                    horizontalParent.Insert(x, slot);
                    VisualElement gridSlotElement = slot.Q<VisualElement>("gridSlot");
                    slotsList.Add(new Slot(new Vector2Int(x,y), gridSlotElement));

                    if (this.gridDataSo.Shape.GetContentFromPosition(new Vector2Int(x, y)))
                    {
                        gridSlotElement.name = $"{x},{y}";
                        RegisterSlotEvents(gridSlotElement, new Vector2Int(x,y));
                        firstFocusElement ??= gridSlotElement;
                    }
                    else
                    {
                        slot.AddToClassList("hiddenSlot");
                    }
                }
            }

            this.slots = slotsList.ToArray();
            return firstFocusElement;
        }

        private void RegisterSlotEvents(VisualElement slot, Vector2Int pos)
        {
            slot.RegisterCallback<FocusEvent>(e => UIManager.Instance.inputsHandler.OnSlotFocus(pos));
        }
        #endregion

        #region CONTENT_SAVE_LOAD
        [Button("Load content"), HideInEditorMode, BoxGroup("Actions")]
        private void LoadContent()
        {
            ClearContent(false);
            foreach (KeyValuePair<Vector2Int, Item> info in gridDataSo.content)
            {
                ItemView itemView = new ItemView(info.Value);
                itemView.GenerateView();
                itemView.gridPosition = info.Key;
                this.slots[GridHelper.GetIndexFromGridPosition(info.Key, gridDataSo.Shape.shapeSize.x)].itemView = itemView;
                AddItem(itemView, info.Key, false);
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
                RemoveItem(itemView);
            
            this.itemsView.Clear();

            if (clearData)
                gridDataSo.content.Clear();
        }
        #endregion

        #region CONTENT_MANAGEMENT
        private void AddItem(ItemView itemView, Vector2Int position, bool addData = true)
        {
            Vector2Int[] itemShapeCoord = itemView.item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            
            int visualPosIndex = GridHelper.GetIndexFromGridPosition(new Vector2Int(position.x, position.y +itemView.item.itemDefinition.Shape.shapeSize.y - 1), gridDataSo.Shape.shapeSize.x);
            this.slots[visualPosIndex].visualElement.Add(itemView.view);
            
            foreach (var coord in itemShapeCoord)
            {
                if (!gridDataSo.Shape.GetContentFromPosition(coord + position))
                    Debug.LogWarning($"try to add item at {coord + position}. This slot doesn't exists.");
                
                int index = GridHelper.GetIndexFromGridPosition(coord + position, gridDataSo.Shape.shapeSize.x);
                this.slots[index].visualElement.AddToClassList("usedSlot");
                this.slots[index].itemView = itemView;
            }
            
            itemView.gridPosition = position;
            this.itemsView.Add(itemView);
            
            if(addData)
                gridDataSo.AddItem(itemView.item, position);
        }
        private void RemoveItem(ItemView itemView, bool removeData = true)
        {
            if (!this.itemsView.Remove(itemView))
            {
                Debug.Log($"Tried to remove {itemView} but it doesn't exists in grid.");
                return;
            }
            
            Vector2Int[] itemShapeCoord = itemView.item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                if (!GridHelper.IsInGrid(coord + itemView.gridPosition, gridDataSo.Shape.shapeSize))
                    Debug.LogWarning($"try to add item at {coord}. This position is out of the grid.");

                this.slots[GridHelper.GetIndexFromGridPosition(coord + itemView.gridPosition, gridDataSo.Shape.shapeSize.x)].visualElement.RemoveFromClassList("usedSlot");
                this.slots[GridHelper.GetIndexFromGridPosition(coord + itemView.gridPosition, gridDataSo.Shape.shapeSize.x)].itemView = null;
            }
            if(removeData)
                gridDataSo.content.Remove(itemView.gridPosition);
        }
        #endregion
    }

    [Serializable]
    public class Slot
    {
        public ItemView itemView;
        public Vector2Int position;
        public VisualElement visualElement;

        public Slot(Vector2Int pos, VisualElement visual)
        {
            this.position = pos;
            this.visualElement = visual;
        }
    }
}
