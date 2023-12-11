using Augment;

namespace Player
{
    using UnityEngine;

    public class AugmentPlayerState : PlayerState
    {
        [SerializeField] private AugmentManager augmentManager;
        public override void OnStateEnter()
        {
            
        }

        public override void Behave<T>(T x)
        {
            if (x is int index)
            {
                augmentManager.AugmentUse(index);
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            }
        }

        public override void OnStateExit()
        {
            
        }
    }
}


