namespace Module.Definitions
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/Definitions/BehaviourModuleDefinition", fileName = "BehaviourModuleDefinition")]
    public class BehaviourModuleDefinitionSO : ModuleDefinitionSO
    {
        public override E_ModuleType ModuleType => E_ModuleType.BEHAVIOUR;
        [SerializeReference]
        public IBehaviourData Behaviour;
    }
    
    public interface IBehaviourData
    { }
    
    [Serializable]
    public struct ProjectileBehaviourData : IBehaviourData
    {
        [SerializeReference]
        public HitShape Shape;
        public float speed;
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
