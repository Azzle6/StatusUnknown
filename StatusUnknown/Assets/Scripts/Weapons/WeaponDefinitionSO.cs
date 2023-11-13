namespace Weapons
{
    using System;
    using Inventory;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/Definitions/WeaponDefinitionSO", fileName = "WeaponDefinition", order = 0)]
    public class WeaponDefinitionSO : ScriptableObject
    {
        public WeaponTriggerData[] triggers;
    }

    [Serializable]
    public struct WeaponTriggerData
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
