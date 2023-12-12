namespace Weapon
{
    using Inventory;
    using Module.Behaviours;
    using Player;
    using UnityEngine;
    using Weapons;

    public abstract class Weapon : MonoBehaviour
    {
        public WeaponManager weaponManager;
        public PlayerInventorySO inventory;
        public WeaponDefinitionSO weaponDefinition;
        public PlayerStat playerStat;
        public Sprite weaponSprite;
        public WeaponType weaponType;
        
        
        public virtual bool ActionPressed() { return false;} 
    
        public abstract void ActionReleased();

        public abstract void Switched(Animator playerAnimator, bool OnOff);
        
        public abstract void Reload(Animator playerAnimator);

        public abstract void AimWithCurrentWeapon();
        
        public abstract void RestWeapon();

        protected void CastModule(E_WeaponOutput trigger, Transform spawnPoint)
        {
            
            WeaponTriggerData data = this.inventory.GetWeaponTriggerData(this.weaponDefinition, trigger);
            if(data == null || data.compiledModules.FirstModule == null)
                return;
            
            ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(
                data.compiledModules.FirstModule,
                new InstantiatedModuleInfo(spawnPoint.position, spawnPoint.rotation));
        }

        public abstract void Hit();
    }

}

public enum WeaponType
{
    MELEE,
    RANGED
}