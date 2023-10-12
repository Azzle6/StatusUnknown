namespace Core.Player
{
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.Serialization;

    public class PlayerStateInterpretor : MonoBehaviour
    {
        
        private Dictionary<string, PlayerState> playerStates = new Dictionary<string, PlayerState>();
        [HideInInspector] public Dictionary<PlayerStateType, PlayerState> statesSlot = new Dictionary<PlayerStateType, PlayerState>();
        [Header("States Slot")]
        public PlayerState movementState;
        public PlayerState aimState;
        public PlayerState actionState;
        [SerializeField] private List<PlayerState> unusedPlayerStates;
        private PlayerState tempState;
        [Header("Player Component")]
        [OdinSerialize] public Rigidbody rb;
        public Animator animator;
        private PlayerAction playerInput;
        
        private bool modifyingState;
        
        private void Awake()
        {
            FillDictionary();
            AddState("IdlePlayerState", PlayerStateType.MOVEMENT);
        }

        private void FillDictionary()
        {
            foreach (PlayerState ps in unusedPlayerStates)
            {
                playerStates.Add(ps.GetType().Name, ps);
            }
            statesSlot.Add(PlayerStateType.ACTION,actionState);
            statesSlot.Add(PlayerStateType.AIM,aimState);
            statesSlot.Add(PlayerStateType.MOVEMENT, movementState);
        }
    
        public void AddState(string state, PlayerStateType playerStateType)
        {
            modifyingState = true;
            tempState = playerStates[state];
            statesSlot[playerStateType] = tempState;
            tempState.OnStateEnter();
            Debug.Log("Added state: " + state);
            modifyingState = false;
        }
    
        public void RemoveState(PlayerStateType playerStateType)
        {
            if (statesSlot[playerStateType] == null)
                return;
                
            modifyingState = true;
            statesSlot[playerStateType].OnStateExit();
            statesSlot[playerStateType] = null;
            modifyingState = false;
        }

        public void LockPlayerInput()
        {
            playerInput.enabled = false;
            RemoveState(PlayerStateType.AIM);
            RemoveState(PlayerStateType.MOVEMENT);
            RemoveState(PlayerStateType.ACTION);
        }

        public void UnlockPlayerInput()
        {
            AddState("IdlePlayerState", PlayerStateType.MOVEMENT);
            playerInput.enabled = true;
        }

        public PlayerState LookForState(string state)
        {
            return playerStates[state];
        }
        
        public void Behave(PlayerStateType type)
        {
            if ((modifyingState) || (statesSlot[type] == null))
                return;
            
            statesSlot[type].Behave();
        }
        
        public void Behave(Vector2 v2, PlayerStateType type)
        {
            if ((modifyingState) || (statesSlot[type] == null))
                return;
            
            statesSlot[type].Behave(v2);
        }
    }
}