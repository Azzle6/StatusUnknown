namespace Module
{
    using Behaviours;
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    public class ModuleCastingTest : MonoBehaviour
    {
        [SerializeField] private PlayerInventorySO inventory;

        [Button, HideInEditorMode]
        private void Cast()
        {
            ModuleBehaviourHandler.InstantiateModuleBehaviour(
                this.inventory.equippedWeaponsData[0].triggerInfoData[0].compiledModules.FirstModule,
                new InstantiatedModuleInfo(transform.position, transform.forward));
        }
    }
}
