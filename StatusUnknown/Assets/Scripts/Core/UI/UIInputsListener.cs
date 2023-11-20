namespace Core.UI
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class UIInputsListener : MonoBehaviour
    {
        [SerializeField, Required]
        private UIInventory inventory;
        private void OnSubmit()
        {
            Debug.Log($"Submit performed.");
        }

        private void OnStart()
        {
            Debug.Log($"Start performed.");
            this.inventory.Display();
            
        }
    }
}
