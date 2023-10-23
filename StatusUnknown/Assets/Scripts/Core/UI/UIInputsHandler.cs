namespace Core.UI
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIInputsHandler : MonoBehaviour
    {
        /*private void Start()
        {
            this.SetFocus(UIManager.Instance.mainUI.rootVisualElement.Q<Button>("first-selected"));
        }*/
        
        /*public void RegisterVisualElementEvents(VisualElement element)
        {
            element.RegisterCallback<FocusEvent>(e => Debug.Log($"Focus on {element.name}."));
        }*/
        
        public void SetFocus(VisualElement element)
        {
            element.Focus();
            Debug.Log($"{element.name} get focus.");
        }
    }
}
