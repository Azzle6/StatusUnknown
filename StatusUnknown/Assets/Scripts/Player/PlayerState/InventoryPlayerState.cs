namespace Player
{
    using Core.EventsSO.GameEventsTypes;
    using UnityEngine;

    public class InventoryPlayerState : PlayerState
    {
        [SerializeField] 
        private BoolGameEvent displayInventoryEvent;

        public override void OnStateEnter()
        {
            Debug.Log("Inventory has been opened player Action are locked");
            this.displayInventoryEvent.RaiseEvent(true);
        }

        public override void OnStateExit()
        {
            Debug.Log("Inventory has been closed player Action are unlocked");
            this.displayInventoryEvent.RaiseEvent(false);
        }
    }

}
