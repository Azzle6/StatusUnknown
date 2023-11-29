namespace Module
{
    using Behaviours;
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    public class ModuleCastingTest : MonoBehaviour
    {
        [SerializeField] private PlayerInventorySO inventory;

        private void CompileModules()
        {
            foreach (var weapon in this.inventory.equippedWeaponsData)
            {
                foreach (var trigger in weapon.triggerInfoData)
                    trigger.compiledModules.CompileWeaponModules(trigger.triggerRowPosition, trigger.modules);
            }
        }
        
        [Button, HideInEditorMode]
        private void Cast()
        {
            this.CompileModules();
            ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(
                this.inventory.equippedWeaponsData[0].triggerInfoData[0].compiledModules.FirstModule,
                new InstantiatedModuleInfo(transform.position, transform.rotation));
        }
        
        
    }
}
