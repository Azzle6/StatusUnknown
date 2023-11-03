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
        public GridElement selectedElement;
        [FoldoutGroup("Dynamic data")]
        public E_NavigationState navigationState;
        
        public void SetFocus(VisualElement element)
        {
            element.Focus();
            Debug.Log($"{element.name} get focus.");
        }

        public void OnElementFocus(GridElement element)
        {
            this.selectedElement = element;
        }

        public void ChangeNavigationState(E_NavigationState state)
        {
            this.navigationState = state;
            Debug.Log($"Change navigation state to : {state}");
        }
    }

    public enum E_NavigationState
    {
        NAVIGATE,
        MOVE_ITEM,
    }
}
