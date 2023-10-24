namespace Core.UI
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIInputsHandler : MonoBehaviour
    {
        
        
        public void SetFocus(VisualElement element)
        {
            element.Focus();
            Debug.Log($"{element.name} get focus.");
        }
    }
}
