namespace Inventory
{
    using Core.SingletonsSO;
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

        [Button("Display")]
        public void DisplayGrid()
        {
            this.GetGridRoot.style.display = DisplayStyle.Flex;
            this.BuildGrid();
        }

        [Button("Hide")]
        public void HideGrid()
        {
            this.GetGridRoot.style.display = DisplayStyle.None;
        }

        private void BuildGrid()
        {
            Shape gridShape = this.gridDataSo.Shape;
            VisualTreeAsset slotTemplate = TestSingletonSO.Instance.slotAsset;//UIManager.Instance.settings.slotTreeAsset;
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
                    
                    if (!this.gridDataSo.Shape.GetContentFromPosition(new Vector2Int(x, y)))
                        slot.AddToClassList("emptySlot");
                    
                    horizontalParent.Insert(x, slot);
                }
            }
        }
    }
}
