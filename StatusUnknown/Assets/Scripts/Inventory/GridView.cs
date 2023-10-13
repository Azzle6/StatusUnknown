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

        private VisualElement gridRoot;

        private VisualElement GetGridRoot
        {
            get
            {
                return this.gridRoot ??= this.uiDocument.rootVisualElement.Q<VisualElement>(this.gridParentName);
            }
        }

        [Button("Display"), HideInEditorMode]
        public void DisplayGrid()
        {
            this.GetGridRoot.style.display = DisplayStyle.Flex;
            VisualElement firstFocus = this.BuildGrid();
            UIManager.Instance.inputsHandler.SetFocus(firstFocus);
        }

        [Button("Hide"), HideInEditorMode]
        public void HideGrid()
        {
            this.GetGridRoot.style.display = DisplayStyle.None;
        }

        private VisualElement BuildGrid()
        {
            VisualElement firstFocusElement = null;
            
            Shape gridShape = this.gridDataSo.Shape;
            VisualTreeAsset slotTemplate = UIManager.Instance.settings.slotTreeAsset;
            VisualElement verticalParent = this.GetGridRoot.Q<VisualElement>("verticalParent");
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
                        UIManager.Instance.inputsHandler.RegisterVisualElementEvents(gridSlotElement);
                        firstFocusElement ??= gridSlotElement;
                    }
                    else
                        slot.AddToClassList("hiddenSlot");
                }
            }
            return firstFocusElement;
        }
    }
}
