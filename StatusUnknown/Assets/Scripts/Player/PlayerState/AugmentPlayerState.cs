namespace Player
{
    using UnityEngine;

    public class AugmentPlayerState : PlayerState
    {
        
        private int lastAugmentIndex;
        public override void OnStateEnter()
        {
            Debug.Log("Augment");
        }

        public void Behave<T>(T x)
        {
            if (x is int index)
            {
                lastAugmentIndex = index;
            }
        }

        public override void OnStateExit()
        {
            
        }
    }
}


