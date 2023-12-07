namespace Core.UI
{
    using EventsSO.GameEventsTypes;
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class UIInputsListener : MonoBehaviour
    {
        [FormerlySerializedAs("inventory")] [SerializeField, Required]
        private BoolGameEvent openInventoryGameEvent;

        [SerializeField] private UIInventoryDisplayer inventoryDisplayer;

        private void OnStart()
        {
            this.openInventoryGameEvent.RaiseEvent(!this.inventoryDisplayer.IsOpen());
        }
    }
}
