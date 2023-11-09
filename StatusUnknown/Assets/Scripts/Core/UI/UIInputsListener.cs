namespace Core.UI
{
    using UnityEngine;

    public class UIInputsListener : MonoBehaviour
    {
        private void OnSubmit()
        {
            Debug.Log($"Submit performed.");
        }

        private void OnStart()
        {
            Debug.Log($"Start performed.");
            
        }
    }
}
