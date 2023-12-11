namespace Module.Definitions
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "CustomAssets/Definitions/EffectorModuleDefinition", fileName = "EffectorModuleDefinition")]
    public class EffectorModuleDefinitionSO : ModuleDefinitionSO
    {
        public override E_ModuleType ModuleType => E_ModuleType.EFFECTOR;
    }
}
