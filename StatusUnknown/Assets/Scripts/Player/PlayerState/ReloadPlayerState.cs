namespace Player
{
using UnityEngine;
    public class ReloadPlayerState : PlayerState
    {
        public override void OnStateEnter()
        {
            Debug.Log("Reload has been started");
        }

        public override void OnStateExit()
        {
            Debug.Log("Reload has been finished");
        }
    }
}


