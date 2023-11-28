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
        public MonoScript projectileBehaviour;

        public LayerMask layerMask;
        public int quantity;
        public Mesh mesh;
        public Material material;
        [SerializeReference]
        public HitShape shape;
        public float speed;
        public float damages;
        public float lifeTime;
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
