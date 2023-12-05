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
        
        
        public abstract void ActionPressed();
    
        public abstract void ActionReleased();

        public abstract void Switched(Animator playerAnimator, bool OnOff);
        
        public abstract void Reload(Animator playerAnimator);

        public abstract void AimWithCurrentWeapon();
        
        public abstract void RestWeapon();

        protected void CastModule(E_WeaponOutput trigger, Transform spawnPoint)
        {
            
            WeaponTriggerData data = this.inventory.GetWeaponTriggerData(this.weaponDefinition, trigger);
            if (data == default)
                return;
            
            this.CompileModules(data);
            ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(
                data.compiledModules.FirstModule,
                new InstantiatedModuleInfo(spawnPoint.position, spawnPoint.rotation));
        }
        
        private void CompileModules(WeaponTriggerData data)
        {
            if (data.compiledModules.FirstModule != null)
                return;
            
            data.compiledModules.CompileWeaponModules(data.triggerRowPosition, data.modules);
        }

        public abstract void Hit();

        
    }

}

public enum WeaponType
{
    MELEE,
    RANGED
}