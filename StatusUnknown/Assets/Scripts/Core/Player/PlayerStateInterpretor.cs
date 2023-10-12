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
        
        
        private void Awake()
        {
            FillDictionary();
            AddState("IdlePlayerState", PlayerStateType.MOVEMENT, true);
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
    
        public void AddState(string state, PlayerStateType playerStateType, bool lockState)
        {
            if (statesSlot[playerStateType] != null)
            {
                if (statesSlot[playerStateType].lockState)
                    return;
            }
            tempState = playerStates[state];
            statesSlot[playerStateType] = tempState;
            statesSlot[playerStateType].lockState = lockState;
            tempState.OnStateEnter();
            Debug.Log("Added state: " + state);
            
        }
    
        public void RemoveState(PlayerStateType playerStateType)
        {
            if (statesSlot[playerStateType] == null)
                return;
            
            statesSlot[playerStateType].OnStateExit();
            statesSlot[playerStateType].lockState = false;
            statesSlot[playerStateType] = null;
        }
        
        public void RemoveStateCheck(string state)
        {
            if (statesSlot[playerStates[state].playerStateType] == null)
                return;
            if (statesSlot[playerStates[state].playerStateType] != playerStates[state])
                return;
            
            statesSlot[playerStates[state].playerStateType].OnStateExit();
            statesSlot[playerStates[state].playerStateType].lockState = false;
            statesSlot[playerStates[state].playerStateType] = null;
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
            AddState("IdlePlayerState", PlayerStateType.MOVEMENT,false);
            playerInput.enabled = true;
        }

        public PlayerState LookForState(string state)
        {
            return playerStates[state];
        }
        
        public void Behave(PlayerStateType type)
        {
            if (statesSlot[type] == null)
                return;
            
            statesSlot[type].Behave();
        }
        
        public void Behave(Vector2 v2, PlayerStateType type)
        {
            if (statesSlot[type] == null)
                return;
            
            statesSlot[type].Behave(v2);
        }
    }
}