using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
        [SerializeField] private BoolGameEvent pauseDisplayGameEvent;

        [SerializeField] private UIInventoryDisplayer inventoryDisplayer;
        [SerializeField] private UIDocument pauseUiDoc;

        private void OnStart()
        {
            this.openInventoryGameEvent.RaiseEvent(!this.inventoryDisplayer.IsOpen());
        }

        private void OnPause()
        {
            pauseDisplayGameEvent.RaiseEvent(pauseUiDoc.rootVisualElement.style.display == DisplayStyle.None);
        }
    }
}
