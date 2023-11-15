namespace Module
{
    using Inventory;
    using Inventory.Item;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Definitions/ModuleDefinition", fileName = "ModuleDefinition")]
    public class ModuleDefinitionSO : GridItemSO
    {
        public override E_ItemType ItemType => E_ItemType.MODULE;
        public Output[] Outputs;
    }
}
