namespace Inventory.Item
{
    using System;
    using Module;
    using Module.Definitions;
    using Weapons;

    [Serializable]
    public abstract class ItemData
    {
        public abstract GridItemSO GridItemDefinition { get; }

        public static ItemData CreateItemData(GridItemSO itemDefinition)
        {
            switch (itemDefinition.ItemType)
            {
                case E_ItemType.MODULE:
                    return new ModuleData((ModuleDefinitionSO)itemDefinition);
                case E_ItemType.WEAPON:
                    return new WeaponData((WeaponDefinitionSO)itemDefinition);
            }
            return null;
        }
    }
}
