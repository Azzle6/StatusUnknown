using System;
using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

namespace Inventory
{
    using Core.UI;
    using Sirenix.OdinInspector;
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

        public Item[] Content
        {
            get => content ??= new Item[gridDataSo.Shape.shapeContent.Length];
            set => content = value;
        }
        [SerializeField]
        private Item[] content;
        
        private VisualElement GridRoot
        {
            get
            {
                return gridRoot ??= uiDocument.rootVisualElement.Q<VisualElement>(this.gridParentName);
            }
        }
        private VisualElement gridRoot;

        #region GRID_BUILD
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

        private VisualElement BuildGrid()
        {
            VisualElement firstFocusElement = null;
            
            Shape gridShape = this.gridDataSo.Shape;
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

                    if (this.gridDataSo.Shape.GetContentFromPosition(new Vector2Int(x, y)))
                    {
                        gridSlotElement.name = $"{x},{y}";
                        firstFocusElement ??= gridSlotElement;
                    }
                    else
                        slot.AddToClassList("hiddenSlot");
                }
            }
            return firstFocusElement;
        }
        #endregion //GRID_BUILD

        #region CONTENT_SAVE_LOAD
        [Button("Load content"), HideInEditorMode, BoxGroup("Actions")]
        private void LoadContent()
        {
            List<Item> result = new List<Item>();
            foreach (KeyValuePair<Vector2Int, ItemSO> info in gridDataSo.content)
            {
                GameObject itemObject = new GameObject($"{info.Value.name} item");
                Item item = itemObject.AddComponent<Item>();
                item.gridPosition = info.Key;
                item.itemDefinition = info.Value;
                result.Add(item);
            }

            Content = result.ToArray();
        }
        
        [Button("Save content"), HideInEditorMode, BoxGroup("Actions")]
        private void SaveContent()
        {
            SerializableDictionary<Vector2Int, ItemSO> newContent = new SerializableDictionary<Vector2Int, ItemSO>();
            foreach (Item item in content.Where(i => i != null))
                newContent.Add(item.gridPosition, item.itemDefinition);
            
            gridDataSo.content = newContent;
        }
        
        [Button("Clear content"), HideInEditorMode, BoxGroup("Actions")]
        private void ClearContent(bool clearData)
        {
            for (int i = 0; i < content.Length; i++)
                Content[i].gridPosition = Vector2Int.zero;

            Content = Array.Empty<Item>();
            
            if(!clearData)
                return;

            gridDataSo.content.Clear();
        }
        #endregion

        #region CONTENT_MANAGEMENT
        private void AddItem(Item item, Vector2Int position)
        {
            Vector2Int[] itemShapeCoord = item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                if (!GridHelper.IsInGrid(coord, gridDataSo.Shape.shapeSize))
                    Debug.LogWarning($"try to add item at {coord}. This position is out of the grid.");

                Content[GridHelper.GetIndexFromGridPosition(coord + position, gridDataSo.Shape.shapeSize.x)] = item;
            }

            item.gridPosition = position;
            gridDataSo.AddItem(item.itemDefinition, position);
        }
        private void RemoveItem(Item item)
        {
            Vector2Int[] itemShapeCoord = item.itemDefinition.Shape.GetPositionsRelativeToAnchor();
            foreach (var coord in itemShapeCoord)
            {
                if (!GridHelper.IsInGrid(coord, gridDataSo.Shape.shapeSize))
                    Debug.LogWarning($"try to add item at {coord}. This position is out of the grid.");

                Content[GridHelper.GetIndexFromGridPosition(coord + item.gridPosition, gridDataSo.Shape.shapeSize.x)] = null;
            }

            gridDataSo.content.Remove(item.gridPosition);
        }
        #endregion
    }
}
