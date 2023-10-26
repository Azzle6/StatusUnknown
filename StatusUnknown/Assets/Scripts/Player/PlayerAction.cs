namespace Core.Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerAction : MonoBehaviour
    {
       [SerializeField] private PlayerInputController playerInputController;
       [SerializeField] private PlayerStateInterpretor playerStateInterpretor;
       [SerializeField] private DeviceLog deviceLog;

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
            playerInputController.OnAimK += OnAimK;
            playerInputController.OnAimG += OnAimG;
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
            playerInputController.OnAimK -= OnAimK;
            playerInputController.OnAimG -= OnAimG;
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
                

            if ((direction == Vector2.zero) && (ctx.canceled))
            {
                playerStateInterpretor.RemoveStateCheck("WalkingPlayerState");
            }
            
            if (direction != Vector2.zero || ctx.performed) 
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.MOVEMENT].name != "WalkingPlayerState")
                    playerStateInterpretor.AddState("WalkingPlayerState",PlayerStateType.MOVEMENT,false);
                playerStateInterpretor.Behave(direction,PlayerStateType.MOVEMENT);
            }
        }
    
        public void OnAimG(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (deviceLog.currentDevice == DeviceType.GAMEPAD) 
            {
                if (ctx.started)
                {
                    playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                    playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                }

                if (ctx.canceled)
                    playerStateInterpretor.RemoveStateCheck("AimGamepadPlayerState");

                if ((ctx.performed) || (direction != Vector2.zero))
                {
                    if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null)
                        playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                    
                    playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                }
            }
            
        }
        public void OnAimK(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (deviceLog.currentDevice == DeviceType.KEYBOARD)
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
            if (ctx.started)
            {
                if (playerStateInterpretor.CheckState(PlayerStateType.ACTION, "InventoryPlayerState"))
                {
                    playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                }
                else
                {
                    playerStateInterpretor.AddState("InventoryPlayerState", PlayerStateType.ACTION,true);
                }
            }

        }
    
        public void OnReload(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                playerStateInterpretor.AddState("ReloadPlayerState", PlayerStateType.ACTION,false);
            }
        
        }
    
        public void OnAugment(InputAction.CallbackContext ctx, int augmentNo)
        {
            if (ctx.started)
            {
                playerStateInterpretor.AddState("AugmentPlayerState", PlayerStateType.ACTION,false);
                playerStateInterpretor.Behave(augmentNo,PlayerStateType.ACTION);
            }

            if (ctx.canceled)
            {
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            }
        }
    
        public void OnWeapon(InputAction.CallbackContext ctx, int weaponNo)
        {
            if (ctx.started)
            {
                playerStateInterpretor.AddState("ShootingPlayerState", PlayerStateType.ACTION,false);
                playerStateInterpretor.Behave(weaponNo,PlayerStateType.ACTION);
            }

            if (ctx.canceled)
            {
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
            }
            
        }
        
    }
}