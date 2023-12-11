namespace Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class PlayerAction : MonoBehaviour
    {
       [SerializeField] private PlayerInputController playerInputController;
       [SerializeField] private PlayerStateInterpretor playerStateInterpretor;
       [SerializeField] private DeviceLog deviceLog;
       [SerializeField] private PlayerStat playerStat;
       private Vector2 mousePos;
       private Vector2 aimDirection;

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
            if (ctx.started)
            {
                playerStateInterpretor.AddState("MovementPlayerState",PlayerStateType.MOVEMENT,false);
                playerStateInterpretor.Behave(direction,PlayerStateType.MOVEMENT);
            }
            
            if ((direction == Vector2.zero) && (ctx.canceled))
            {
                playerStateInterpretor.RemoveStateCheck("MovementPlayerState");
            }
            
            if (direction != Vector2.zero || ctx.performed)
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.MOVEMENT].name != "MovementPlayerState")
                    playerStateInterpretor.AddState("MovementPlayerState",PlayerStateType.MOVEMENT,false);
                playerStateInterpretor.Behave(direction,PlayerStateType.MOVEMENT);

                if (deviceLog.currentDevice == DeviceType.GAMEPAD)
                {
                    if ((aimDirection.magnitude < 0.1f) && (playerStateInterpretor.statesSlot[PlayerStateType.AIM] != null))
                    {
                        playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                    }
                }
            }
        }
    
        public void OnAimG(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (deviceLog.currentDevice == DeviceType.KEYBOARD) 
                return;
            
            aimDirection = direction;
                
            if (ctx.started)
            {
                playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
                playerStat.isAiming = true;
            }

            if ((ctx.canceled) && (playerStat.isShooting == false) || (direction == Vector2.zero) && (playerStat.isShooting == false))
            {
                playerStateInterpretor.RemoveStateCheck("AimGamepadPlayerState");
                playerStat.isAiming = false;
            }
            
                
            if ((ctx.performed) || (direction != Vector2.zero))
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null)
                {
                    playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                }

             
                playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
            }
        }
        
        public void OnAimK(Vector2 direction, InputAction.CallbackContext ctx)
        {
            if (deviceLog.currentDevice == DeviceType.GAMEPAD)
                return;

            if (ctx.started)
            {
                playerStateInterpretor.AddState("AimMousePlayerState",PlayerStateType.AIM,false);
            }

            if (ctx.canceled)
            {
                playerStateInterpretor.RemoveStateCheck("AimMousePlayerState");
            }
               
            playerStateInterpretor.Behave(direction,PlayerStateType.AIM);

            if (ctx.performed)
            {
                mousePos = direction;
                /*if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null)
                    playerStateInterpretor.AddState("AimMousePlayerState",PlayerStateType.AIM,false);*/
                playerStateInterpretor.Behave(direction,PlayerStateType.AIM);
            }
            
        }
    
        public void OnMedkit(InputAction.CallbackContext ctx)
        {            
            if (ctx.started)
            {
                playerStateInterpretor.AddState("MedikitPlayerState", PlayerStateType.ACTION,false);
            }
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
            if ((ctx.started) && (!playerStat.isShooting))
            {
                playerStateInterpretor.AddState("ReloadPlayerState", PlayerStateType.ACTION,false);
            }
        
        }
    
        public void OnAugment(InputAction.CallbackContext ctx, int augmentNo)
        {
            if (ctx.started)
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.ACTION] != null)
                    playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                
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
                if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null)
                {
                    if (deviceLog.currentDevice == DeviceType.GAMEPAD)
                    {
                        if (aimDirection == Vector2.zero)
                        {
                            playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                            playerStateInterpretor.Behave(aimDirection,PlayerStateType.AIM);
                        }
                    }
                    
                    
                    if (deviceLog.currentDevice == DeviceType.KEYBOARD)
                    {
                        playerStateInterpretor.AddState("AimMousePlayerState",PlayerStateType.AIM,false);
                        playerStateInterpretor.Behave(mousePos,PlayerStateType.AIM);
                    }
            
                }
                    
                playerStateInterpretor.AddState("ShootingPlayerState", PlayerStateType.ACTION,false);
                playerStateInterpretor.Behave(weaponNo,PlayerStateType.ACTION);
            }

            if (ctx.canceled)
            {
                if (!playerStat.currentWeaponIsMelee)
                    playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                
                if (deviceLog.currentDevice == DeviceType.GAMEPAD)
                {
                    if (aimDirection != Vector2.zero)
                    {
                        playerStateInterpretor.AddState("AimGamepadPlayerState",PlayerStateType.AIM,false);
                        playerStateInterpretor.Behave(aimDirection,PlayerStateType.AIM);
                    }
                    else
                    {
                        playerStateInterpretor.RemoveState(PlayerStateType.AIM);
                    }
                }
            }
            
        }
        
    }
}