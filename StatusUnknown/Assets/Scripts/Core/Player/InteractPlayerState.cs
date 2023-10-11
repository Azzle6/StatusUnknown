namespace Core.Player
{
    using UnityEngine;

    public class InteractPlayerState : PlayerState
    {
        public override void OnStateEnter()
        {
            Debug.Log("Player tried to interact");
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
        }

        public override void OnStateExit()
        {
        }
    }

}

