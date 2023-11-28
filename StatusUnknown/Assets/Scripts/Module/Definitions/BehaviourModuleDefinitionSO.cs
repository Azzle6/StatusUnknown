namespace Module.Definitions
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Definitions/BehaviourModuleDefinition", fileName = "BehaviourModuleDefinition")]
    public class BehaviourModuleDefinitionSO : ModuleDefinitionSO
    {
        public override E_ModuleType ModuleType => E_ModuleType.BEHAVIOUR;
        [SerializeReference]
        public IBehaviourData BehaviourData;
    }
    
    public interface IBehaviourData
    { }
    
    [Serializable]
    public struct ProjectileBehaviourData : IBehaviourData
    {
        [SerializeField]
        public MonoScript behaviour;
    }

    [Serializable]
    public struct ZoneBehaviourData : IBehaviourData
    {
        [SerializeReference]
        public HitShape Shape;
    }

    [Serializable]
    public struct DropBehaviourData : IBehaviourData
    {
        [Required]
        public GameObject instantiatedObject;
    }
}
