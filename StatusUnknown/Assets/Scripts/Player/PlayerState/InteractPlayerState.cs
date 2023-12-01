
namespace Player
{
    using System.Collections; 
    using UnityEngine;

    public class InteractPlayerState : PlayerState
    {
        private float holdTimer;
        private Collider[] interactables;
        public override void OnStateEnter()
        {
            StopAllCoroutines();
            holdTimer = 0;
            interactables = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider interactable in interactables)
            {
                if (interactable.TryGetComponent(out IInteractable iInteractable))
                {
                    iInteractable.Interact();
                }
            }
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

