namespace Player
{
    using UnityEngine;

    public abstract class PlayerState : MonoBehaviour
    {
        [SerializeField] protected PlayerStateInterpretor playerStateInterpretor;
        public PlayerStateType playerStateType;
        [HideInInspector] public bool lockState;

        public abstract void OnStateEnter();
    
        public virtual void Behave() {}
        public virtual void Behave<T>(T x) {} 
    
        public abstract void OnStateExit();

    }
    
    public enum PlayerStateType
    {
        MOVEMENT,
        AIM,
        ACTION
    }
}