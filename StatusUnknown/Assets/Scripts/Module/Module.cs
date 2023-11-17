namespace Module
{
    using System;
    using Inventory.Item;

    [Serializable]
    public class Module : Item
    {
        public override GridItemSO GridItemDefinition => this.definition;
        public ModuleDefinitionSO definition;

        public Module(ModuleDefinitionSO definition)
        {
            this.definition = definition;
        }
    }
}

