namespace Core.Player
{
    using UnityEngine;

    public abstract class PlayerState : MonoBehaviour
    {
        [SerializeField] protected PlayerStateInterpretor playerStateInterpretor;
        public PlayerStateType playerStateType;
        public bool lockState;

        public abstract void OnStateEnter();
    
        public virtual void Behave() {}
        public virtual void Behave(Vector2 v2) {}
    
        public abstract void OnStateExit();

    }
    
    public enum PlayerStateType
    {
        MOVEMENT,
        AIM,
        ACTION
    }
}