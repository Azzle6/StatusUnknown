namespace Inventory
{
    using AYellowpaper.SerializedCollections;
    using Core.Helpers;
    using Core.UI;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private HashSet<Item> items = new HashSet<Item>();
        
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
            VisualTreeAsset slotTemplate = UIManager.Instance.settings.slotTreeAsset;
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
            ClearContent(true);
            foreach (KeyValuePair<Vector2Int, Item> info in gridDataSo.content)
            {
                this.slots[GridHelper.GetIndexFromGridPosition(info.Key, gridDataSo.Shape.shapeSize.x)].item = info.Value;
                AddItem(info.Value, info.Key);
            }
        }
        
        [Button("Save content"), HideInEditorMode, BoxGroup("Actions")]
        private void SaveContent()
        {
            SerializedDictionary<Vector2Int, Item> newContent = new SerializedDictionary<Vector2Int, Item>();
            foreach (Item item in this.items)
                newContent.Add(item.gridPosition, item);
            
            gridDataSo.content = newContent;
        }
        
        [Button("Clear content"), HideInEditorMode, BoxGroup("Actions")]
        private void ClearContent(bool clearData)
        {
            HashSet<Item> tempItem = new HashSet<Item>(this.items);
            foreach (Item item in tempItem)
                RemoveItem(item);
            
            this.items.Clear();

            if (clearData)
                gridDataSo.content.Clear();
        }
        #endregion

        #region CONTENT_MANAGEMENT
        private void AddItem(Item item, Vector2Int position)
        {
            Vector2Int[] itemShapeCoord = item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            int posIndex = GridHelper.GetIndexFromGridPosition(position, gridDataSo.Shape.shapeSize.x);
            //[TODO]Spawn item here
            
            foreach (var coord in itemShapeCoord)
            {
                if (!GridHelper.IsInGrid(coord + position, gridDataSo.Shape.shapeSize))
                    Debug.LogWarning($"try to add item at {coord}. This position is out of the grid.");
                int index = GridHelper.GetIndexFromGridPosition(coord + position, gridDataSo.Shape.shapeSize.x);
                this.slots[index].visualElement.AddToClassList("usedSlot");
                this.slots[index].item = item;
            }
            
            item.gridPosition = position;
            this.items.Add(item);
            gridDataSo.AddItem(item);
        }
        private void RemoveItem(Item item)
        {
            if (!this.items.Remove(item))
            {
                Debug.Log($"Tried to remove {item} but it doesn't exists in grid.");
                return;
            }
            
            Vector2Int[] itemShapeCoord = item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                if (!GridHelper.IsInGrid(coord + item.gridPosition, gridDataSo.Shape.shapeSize))
                    Debug.LogWarning($"try to add item at {coord}. This position is out of the grid.");

                this.slots[GridHelper.GetIndexFromGridPosition(coord + item.gridPosition, gridDataSo.Shape.shapeSize.x)].visualElement.RemoveFromClassList("usedSlot");
                this.slots[GridHelper.GetIndexFromGridPosition(coord + item.gridPosition, gridDataSo.Shape.shapeSize.x)].item = null;
            }
            gridDataSo.content.Remove(item.gridPosition);
        }
        #endregion
    }

    [Serializable]
    public class Slot
    {
        public Item item;
        public Vector2Int position;
        public VisualElement visualElement;

        public Slot(Vector2Int pos, VisualElement visual)
        {
            this.position = pos;
            this.visualElement = visual;
        }
    }
}
