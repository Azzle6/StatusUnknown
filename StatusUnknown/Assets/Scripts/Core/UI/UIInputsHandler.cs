namespace Core.UI
{
    using Inventory;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIInputsHandler : MonoBehaviour
    {
        public Vector2Int selectedPosition;
        public Item selectedItem;
        
        public void SetFocus(VisualElement element)
        {
            element.Focus();
            Debug.Log($"{element.name} get focus.");
        }

        public void OnSlotFocus(Vector2Int pos)
        {
            this.selectedPosition = pos;
        }
    }
}
