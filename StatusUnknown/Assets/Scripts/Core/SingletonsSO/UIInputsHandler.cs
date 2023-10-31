namespace Core.SingletonsSO
{
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/UIHandler", fileName = "UIHandler")]
    public class UIInputsHandler : SingletonSO<UIInputsHandler>
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

        public void OnSlotFocus(GridElement element)
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
