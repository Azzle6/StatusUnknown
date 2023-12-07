namespace Core.UI
{
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class UIInputsListener : MonoBehaviour
    {
        [FormerlySerializedAs("inventory")] [SerializeField, Required]
        private UIInventoryDisplayer inventoryDisplayer;

        /*private void OnStart()
        {
            this.inventoryDisplayer.Display();
        }*/
    }
}
