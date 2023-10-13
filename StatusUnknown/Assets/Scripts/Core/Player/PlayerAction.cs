namespace Core.Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerAction : MonoBehaviour
    {
       [SerializeField] private PlayerInputController playerInputController;
       [SerializeField] private PlayerStateInterpretor playerStateInterpretor;

        private void OnEnable()
        {
            EnableEvent();
        }

        private void OnDisable()
        {
            DisableEvent();
        }

        private void EnableEvent()
        {
            playerInputController.OnMovement += OnMove;
            playerInputController.OnAim += OnAim;
            playerInputController.OnMedkit += OnMedkit;
            playerInputController.OnInteract += OnInteract;
            playerInputController.OnInventory += OnInventory;
            playerInputController.OnReload += OnReload;
            playerInputController.OnAugment += OnAugment;
            playerInputController.OnWeapon += OnWeapon;
        }

        private void DisableEvent()
        {
            playerInputController.OnMovement -= OnMove;
            playerInputController.OnAim -= OnAim;
            playerInputController.OnMedkit -= OnMedkit;
            playerInputController.OnInteract -= OnInteract; 
            playerInputController.OnInventory -= OnInventory;
            playerInputController.OnReload -= OnReload;
            playerInputController.OnAugment -= OnAugment;
            playerInputController.OnWeapon -= OnWeapon;
        }
        
        
        public void OnMove(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (ctx.started && direction.magnitude > 0.1f)
                playerStateInterpretor.AddState("WalkingPlayerState",PlayerStateType.MOVEMENT,false);
                

            if ((direction == Vector2.zero) && (!ctx.performed) && (ctx.canceled))
            {
                playerStateInterpretor.RemoveStateCheck("WalkingPlayerState");
            }
            
            if (direction != Vector2.zero) 
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.MOVEMENT] == null)
                    playerStateInterpretor.AddState("WalkingPlayerState",PlayerStateType.MOVEMENT,false);

                playerStateInterpretor.Behave(direction,PlayerStateType.MOVEMENT);
            }
        }
    
        public void OnAim(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (DeviceLog.Instance.currentDevice == Mouse.current)
            {
                if (ctx.started)
                {
                    playerStateInterpretor.AddState("AimMousePlayerState",PlayerStateType.AIM,false);
                    playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                }
            
                if (ctx.canceled)
                    playerStateInterpretor.RemoveStateCheck("AimMousePlayerState");
            
                if (ctx.performed)
                {
                    if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null)
                        playerStateInterpretor.AddState("AimMousePlayerState",PlayerStateType.AIM,false);
                
                    playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                }
            }

            if (DeviceLog.Instance.currentDevice == Gamepad.current) 
            {
         
            }
            
        }
    
        public void OnMedkit(InputAction.CallbackContext ctx)
        {            
        }
    
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
                playerStateInterpretor.AddState("InteractPlayerState", PlayerStateType.ACTION,false);

            if (ctx.canceled)
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
        
        }
    
        public void OnInventory(InputAction.CallbackContext ctx)
        {

        }
    
        public void OnReload(InputAction.CallbackContext ctx)
        {
        
        }
    
        public void OnAugment(InputAction.CallbackContext ctx, int augmentNo)
        {
        
        }
    
        public void OnWeapon(InputAction.CallbackContext ctx, int weaponNo)
        {
        }
        
    }
}