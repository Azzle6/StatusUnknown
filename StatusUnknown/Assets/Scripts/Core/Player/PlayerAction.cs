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
                playerStateInterpretor.AddState("WalkingPlayerState",PlayerStateType.MOVEMENT);
                

            if ((direction == Vector2.zero) && (!ctx.performed) && (ctx.canceled))
            {
                playerStateInterpretor.RemoveState(PlayerStateType.MOVEMENT);
            }
            
            if (direction != Vector2.zero) 
            {
                if (playerStateInterpretor.statesSlot[PlayerStateType.MOVEMENT] == null)
                    playerStateInterpretor.AddState("WalkingPlayerState",PlayerStateType.MOVEMENT);
                
                playerStateInterpretor.Behave(direction,PlayerStateType.MOVEMENT);
                Debug.Log(ctx.control.device.name);
            }
            CheckForLastDevice(ctx);
        }
    
        public void OnAim(Vector2 direction, InputAction.CallbackContext ctx)
        {
            CheckForLastDevice(ctx);
        }
    
        public void OnMedkit(InputAction.CallbackContext ctx)
        {            
            CheckForLastDevice(ctx);
        }
    
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
                playerStateInterpretor.AddState("InteractPlayerState", PlayerStateType.ACTION);
            CheckForLastDevice(ctx);
        
        }
    
        public void OnInventory(InputAction.CallbackContext ctx)
        {
            CheckForLastDevice(ctx);

        }
    
        public void OnReload(InputAction.CallbackContext ctx)
        {
            CheckForLastDevice(ctx);
        
        }
    
        public void OnAugment(InputAction.CallbackContext ctx, int augmentNo)
        {
            CheckForLastDevice(ctx);
        
        }
    
        public void OnWeapon(InputAction.CallbackContext ctx, int weaponNo)
        {
            CheckForLastDevice(ctx);
        }

        private void CheckForLastDevice(InputAction.CallbackContext ctx)
        {
            DeviceLog.Instance.currentDevice = ctx.control.device;
        }
    }
}