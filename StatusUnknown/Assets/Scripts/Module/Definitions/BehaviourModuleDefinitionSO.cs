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
        [BoxGroup("Stats"), SerializeReference]
        public IBehaviourData BehaviourData;
    }

    public interface IBehaviourData
    {
        public MonoScript Behaviour { get; set; }
        public int Quantity { get; set; }
        public IInstantiationRule InstantiationRule { get; set; }
        public float TickRate { get; set; }
        public LayerMask LayerMask { get; set; }
        public HitShape HitShape { get; set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
    }
    
    [Serializable]
    public struct ProjectileBehaviourData : IBehaviourData
    {
        [BoxGroup("General stats")][Tooltip("Type -1 to disable Ticker.")] [field: SerializeField]
        public float TickRate { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public MonoScript Behaviour { get; set; }
        
        [BoxGroup("Instantiation")] [field: SerializeField]
        public int Quantity { get; set; }
        [BoxGroup("Instantiation")] [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [BoxGroup("General stats")][field:SerializeReference]
        public HitShape HitShape { get; set; }
        
        [BoxGroup("Visual")] [field: SerializeField]
        public Mesh Mesh { get; set; }
        [BoxGroup("Visual")] [field: SerializeField]
        public Material Material { get; set; }
        
        [BoxGroup("Definition")] 
        public float speed;
        [BoxGroup("Definition")]
        public float damages;
    }

    [Serializable]
    public struct ZoneBehaviourData : IBehaviourData
    {
        [BoxGroup("General stats")][Tooltip("Type -1 to disable Ticker.")] [field: SerializeField]
        public float TickRate { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public MonoScript Behaviour { get; set; }
        
        [BoxGroup("Instantiation")] [field: SerializeField]
        public int Quantity { get; set; }
        [BoxGroup("General stats")] [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [BoxGroup("General stats")] [field:SerializeReference]
        public HitShape HitShape { get; set; }
        
        [BoxGroup("Visual")] [field: SerializeField]
        public Mesh Mesh { get; set; }
        [BoxGroup("Visual")] [field: SerializeField]
        public Material Material { get; set; }

        public float damages;
    }

    public struct ElementPositionInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public ElementPositionInfo(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }
}
