using UnityEngine.Serialization;

namespace Player
{
    using System.Collections.Generic;
    using UnityEngine;
    using Weapon;

    public class PlayerStateInterpretor : MonoBehaviour
    {
        
        private Dictionary<string, PlayerState> playerStates = new Dictionary<string, PlayerState>();
        public Dictionary<PlayerStateType, PlayerState> statesSlot = new Dictionary<PlayerStateType, PlayerState>();
        
        [HideInInspector] public PlayerState movementState;
        [HideInInspector] public PlayerState aimState;
        [HideInInspector] public PlayerState actionState;
        [HideInInspector] public PlayerState inputBufferState;
        //[HideInInspector] public string inputBufferStateName;
        [SerializeField] private List<PlayerState> unusedPlayerStates;
        private PlayerState tempState;
        [Header("Player Component")]
        public Rigidbody rb;
        public Animator animator;
        public WeaponManager weaponManager;
        private PlayerAction playerInput;
        
        
        private void Awake()
        {
            FillDictionary();
            AddState("IdlePlayerState", PlayerStateType.MOVEMENT, false);
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
    
        //before adding a state need to remove previous state
        public void AddState(string state, PlayerStateType playerStateType, bool lockState)
        {
            if (statesSlot[playerStateType] != null)
            {
                if (statesSlot[playerStateType].inputBufferActive)
                    inputBufferState = playerStates[state];
                
                if (statesSlot[playerStateType].lockState)
                    return;
            }
            
            tempState = playerStates[state];
            statesSlot[playerStateType] = tempState;
            statesSlot[playerStateType].lockState = lockState;
            tempState.OnStateEnter();

        }
    
        public void RemoveState(PlayerStateType playerStateType)
        {
            if (statesSlot[playerStateType] == default)
                return;
            
            tempState = statesSlot[playerStateType];
            tempState.OnStateExit();
            statesSlot[playerStateType].lockState = false;
            statesSlot[playerStateType] = null;
        }
        public bool CheckState(PlayerStateType playerStateType, string playerStateName)
        {
            if (statesSlot[playerStateType] == null)  
                return false;
            if (statesSlot[playerStateType] != null)
            {
                if (statesSlot[playerStateType].GetType().Name != playerStateName)
                    return false;
            }
            return true;
        }
        public void RemoveStateCheck(string state)
        {
            if (statesSlot[playerStates[state].playerStateType] == null)
                return;
            if (statesSlot[playerStates[state].playerStateType] != playerStates[state])
                return;
            tempState = statesSlot[playerStates[state].playerStateType];
            statesSlot[playerStates[state].playerStateType].lockState = false;
            statesSlot[playerStates[state].playerStateType] = null;
            tempState.OnStateExit();
        }

        public void ExecuteBufferInput()
        {
            Debug.Log("Buffer Executing");
            if(inputBufferState == default)
                return;
            AddState(inputBufferState.GetType().Name, inputBufferState.playerStateType, false);
            inputBufferState = default;
            Debug.Log("Buffer Executed");
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
        
        public void Behave<T>(T x, PlayerStateType type)
        {
            if (statesSlot[type] == default)
                return;
            
            statesSlot[type].Behave<T>(x);
        }
    }
}