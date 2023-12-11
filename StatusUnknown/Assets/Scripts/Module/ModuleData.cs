namespace Module
{
    using System;
    using Definitions;
    using Inventory.Item;

    [Serializable]
    public class ModuleData : ItemData
    {
        public override GridItemSO GridItemDefinition => this.definition;
        public ModuleDefinitionSO definition;

        public ModuleData(ModuleDefinitionSO definition)
        {
            this.definition = definition;
        }
    }
}

