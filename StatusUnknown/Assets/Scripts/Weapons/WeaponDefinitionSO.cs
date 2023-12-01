namespace Weapons
{
    using System;
    using Inventory;
    using Inventory.Item;
    using Module;
    using UnityEngine;
    using UnityEngine.Serialization;

    [CreateAssetMenu(menuName = "CustomAssets/Definitions/WeaponDefinition", fileName = "WeaponDefinition")]
    public class WeaponDefinitionSO : GridItemSO
    {
        public override E_ItemType ItemType => E_ItemType.WEAPON;
        public WeaponTriggerDefinition[] triggers;
    }

    [Serializable]
    public struct WeaponTriggerDefinition
    {
        [FormerlySerializedAs("trigger")] public WeaponTriggerSO weaponTrigger;
        public int triggerRowPosition;
        public Shape shape;
    }
}
