namespace Core.SingletonsSO
{
    using Inventory;
    using Sirenix.OdinInspector;
    using UI;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/UIHandler", fileName = "UIHandler")]
    public class UIHandler : SingletonSO<UIHandler>
    {
        public UISettings uiSettings;
        
        [FoldoutGroup("Dynamic data")]
        public GridView selectedGrid;
        [FoldoutGroup("Dynamic data")]
        public bool isMovingItem;
        [FoldoutGroup("Dynamic data")]
        public ItemView movingItem;
        
        public void ForceFocus(VisualElement element)
        {
            element.Focus();
            Debug.Log($"{element.name} get forced focus.");
        }

        public void OnGridFocus(GridView grid)
        {
            this.selectedGrid = grid;
        }

        public void OnPickItem(ItemView itemView)
        {
            this.movingItem = itemView;
            this.isMovingItem = true;
        }

        public void OnDropItem()
        {
            this.isMovingItem = false;
        }
    }
}
