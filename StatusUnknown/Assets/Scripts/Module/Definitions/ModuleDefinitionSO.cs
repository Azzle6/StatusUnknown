namespace Module.Definitions
{
    using System;
    using Core.Helpers;
    using Inventory.Item;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class ModuleDefinitionSO : GridItemSO
    {
        public override E_ItemType ItemType => E_ItemType.MODULE;
        public abstract E_ModuleType ModuleType { get; }
        public Output[] outputs;
    }

    public enum E_ModuleType
    {
        BEHAVIOUR,
        EFFECTOR,
        PASSIVE,
    }

    public enum E_ModuleOutput
    {
        ON_SPAWN,
        ON_HIT,
        ON_TICK,
        ON_END
    }
    
    [Serializable]
    public struct Output
    {
        public E_ModuleOutput moduleTriggerType;
        public Vector2Int localPosition;
        public E_Direction direction;
    }
}
