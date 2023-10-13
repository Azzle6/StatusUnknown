namespace Core.Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInputAction playerInput;
        
        
        public delegate void MovementEvent(Vector2 movement, InputAction.CallbackContext ctx);
        public event MovementEvent OnMovement;
        
        public delegate void AimKEvent(Vector2 aimDir, InputAction.CallbackContext ctx);
        public event AimKEvent OnAimK;
        
        public delegate void AimGEvent(Vector2 aimDir, InputAction.CallbackContext ctx);
        public event AimGEvent OnAimG;
        
        public delegate void MedkitEvent(InputAction.CallbackContext ctx);
        public event MedkitEvent OnMedkit;
        
        public delegate void InteractEvent(InputAction.CallbackContext ctx);
        public event InteractEvent OnInteract;
        
        public delegate void InventoryEvent(InputAction.CallbackContext ctx);
        public event InventoryEvent OnInventory;
        
        public delegate void ReloadEvent(InputAction.CallbackContext ctx);
        public event ReloadEvent OnReload;
        
        public delegate void AugmentEvent(InputAction.CallbackContext ctx, int augmentNo);
        public event AugmentEvent OnAugment;
        
        public delegate void WeaponEvent(InputAction.CallbackContext ctx, int weaponNo);
        public event WeaponEvent OnWeapon;

        private void Awake()
        {
            playerInput = new PlayerInputAction();
            playerInput.Enable();
        }

        private void OnEnable()
        {
            EnableBindings();
        }
        
        private void EnableBindings()
        {
            playerInput.PlayerActionMaps.Movement.started += ctx => Movement(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.Movement.canceled += ctx => Movement(ctx.ReadValue<Vector2>(), ctx);
            
            playerInput.PlayerActionMaps.AimK.started += ctx => AimK(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.AimK.performed += ctx => AimK(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.AimK.canceled += ctx => AimK(ctx.ReadValue<Vector2>(), ctx);
            
            playerInput.PlayerActionMaps.AimG.started += ctx => AimG(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.AimG.performed += ctx => AimG(ctx.ReadValue<Vector2>(), ctx);
            playerInput.PlayerActionMaps.AimG.canceled += ctx => AimG(ctx.ReadValue<Vector2>(), ctx);
            
            playerInput.PlayerActionMaps.Medkit.started += ctx => Medkit(ctx);
            playerInput.PlayerActionMaps.Medkit.performed += ctx => Medkit(ctx);
            playerInput.PlayerActionMaps.Medkit.canceled += ctx => Medkit(ctx);
            
            playerInput.PlayerActionMaps.Interact.started += ctx => Interact(ctx);
            playerInput.PlayerActionMaps.Interact.performed += ctx => Interact(ctx);
            playerInput.PlayerActionMaps.Interact.canceled += ctx => Interact(ctx);
            
            playerInput.PlayerActionMaps.Inventory.started += ctx => Inventory(ctx);
            playerInput.PlayerActionMaps.Inventory.performed += ctx => Inventory(ctx);
            playerInput.PlayerActionMaps.Inventory.canceled += ctx => Inventory(ctx);
            
            playerInput.PlayerActionMaps.Reload.started += ctx => Reload(ctx);
            playerInput.PlayerActionMaps.Reload.performed += ctx => Reload(ctx);
            playerInput.PlayerActionMaps.Reload.canceled += ctx => Reload(ctx);
            
            playerInput.PlayerActionMaps.Augment1.started += ctx => Augment(ctx, 0);
            playerInput.PlayerActionMaps.Augment1.performed += ctx => Augment(ctx, 0);
            playerInput.PlayerActionMaps.Augment1.canceled += ctx => Augment(ctx, 0);
            
            playerInput.PlayerActionMaps.Augment2.started += ctx => Augment(ctx, 1);
            playerInput.PlayerActionMaps.Augment2.performed += ctx => Augment(ctx, 1);
            playerInput.PlayerActionMaps.Augment2.canceled += ctx => Augment(ctx, 1);
            
            playerInput.PlayerActionMaps.Augment3.started += ctx => Augment(ctx, 2);
            playerInput.PlayerActionMaps.Augment3.performed += ctx => Augment(ctx, 2);
            playerInput.PlayerActionMaps.Augment3.canceled += ctx => Augment(ctx, 2);
            
            playerInput.PlayerActionMaps.Weapon1.started += ctx => Weapon(ctx, 0);
            playerInput.PlayerActionMaps.Weapon1.performed += ctx => Weapon(ctx, 0);
            playerInput.PlayerActionMaps.Weapon1.canceled += ctx => Weapon(ctx, 0);
            
            playerInput.PlayerActionMaps.Weapon2.started += ctx => Weapon(ctx, 1);
            playerInput.PlayerActionMaps.Weapon2.performed += ctx => Weapon(ctx, 1);
            playerInput.PlayerActionMaps.Weapon2.canceled += ctx => Weapon(ctx, 1);
        }
        

        private void Movement(Vector2 movement, InputAction.CallbackContext ctx)
        {
            if (OnMovement != null)
                OnMovement(movement, ctx);
        }
        
        private void AimK(Vector2 aimDir, InputAction.CallbackContext ctx)
        {
            OnAimK?.Invoke(aimDir, ctx);
        }
        
        private void AimG(Vector2 aimDir, InputAction.CallbackContext ctx)
        {
            OnAimG?.Invoke(aimDir, ctx);
        }
        
        private void Medkit(InputAction.CallbackContext ctx)
        {
            OnMedkit?.Invoke(ctx);
        }
        
        private void Interact(InputAction.CallbackContext ctx)
        {
            OnInteract?.Invoke(ctx);
        }
        
        private void Inventory(InputAction.CallbackContext ctx)
        {
            OnInventory?.Invoke(ctx);
        }
        
        private void Reload(InputAction.CallbackContext ctx)
        {
            OnReload?.Invoke(ctx);
        }
        
        private void Augment(InputAction.CallbackContext ctx, int augmentNo)
        {
            OnAugment?.Invoke(ctx, augmentNo);
        }
        
        private void Weapon(InputAction.CallbackContext ctx, int weaponNo)
        {
            OnWeapon?.Invoke(ctx, weaponNo);
        }
    }
}

