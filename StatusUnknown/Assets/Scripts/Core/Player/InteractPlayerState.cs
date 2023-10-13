using System.Collections;

namespace Core.Player
{
    using UnityEngine;

    public class InteractPlayerState : PlayerState
    {
        private float holdTimer;
        public override void OnStateEnter()
        {
            holdTimer = 0;
            Debug.Log("Player tried to interact");
        }

        private IEnumerator HoldTimer(float holdMax)
        {
            while (holdTimer < holdMax)
            {
                holdTimer += Time.deltaTime;
                yield return null;
            }
            //Iinteractable.Interact();
        }
        

        public override void OnStateExit()
        {
            
        }
    }

}

