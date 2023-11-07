namespace Player
{
    using UnityEngine;

    public class InventoryPlayerState : PlayerState
    {
        

        public override void OnStateEnter()
        {
            Debug.Log("Inventory has been opened player Action are locked");
        }

        public override void OnStateExit()
        {
            Debug.Log("Inventory has been closed player Action are unlocked");
        }
    }

}
