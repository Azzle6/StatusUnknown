namespace Module
{
    using System;
    using Core.Helpers;
    using Inventory.Item;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Definitions/ModuleDefinition", fileName = "ModuleDefinition")]
    public class ModuleDefinitionSO : GridItemSO
    {
        public override E_ItemType ItemType => E_ItemType.MODULE;
        public Output[] outputs;
    }
    
    [Serializable]
    public struct Output
    {
        public TriggerSO triggerType;
        public Vector2Int localPosition;
        public E_Direction direction;
    }
}
