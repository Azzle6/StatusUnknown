namespace Weapons
{
    using System;
    using Inventory;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/Definitions/WeaponDefinitionSO", fileName = "WeaponDefinition", order = 0)]
    public class WeaponDefinitionSO : ScriptableObject
    {
        public string weaponName;
        public WeaponTriggerDefinition[] triggers;
    }

    [Serializable]
    public struct WeaponTriggerDefinition
    {
        public E_TriggerType trigger;
        public Shape shape;
    }

    public enum E_TriggerType
    {
        ON_FIRE,
        ON_RELOAD
    }
}
