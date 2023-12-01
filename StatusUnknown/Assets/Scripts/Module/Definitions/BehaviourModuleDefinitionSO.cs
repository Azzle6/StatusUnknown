namespace Module.Definitions
{
    using System;
    using Core.Helpers;
    using Sirenix.OdinInspector;
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
        public ScriptReference Behaviour { get; set; }
        public int Quantity { get; set; }
        public IInstantiationRule InstantiationRule { get; set; }
        public float TickRate { get; set; }
        public LayerMask LayerMask { get; set; }
        public HitShape CollisionShape { get; set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        public float Damages { get; set; }
    }
    
    [Serializable]
    public struct ProjectileBehaviourData : IBehaviourData
    {
        [BoxGroup("General stats")][Tooltip("Type 0 to disable Ticker.")] [field: SerializeField]
        public float TickRate { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public ScriptReference Behaviour { get; set; }
        
        [BoxGroup("Instantiation")] [field: SerializeField]
        public int Quantity { get; set; }
        [BoxGroup("Instantiation")] [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [BoxGroup("General stats")][field:SerializeReference]
        public HitShape CollisionShape { get; set; }
        
        [BoxGroup("Visual")] [field: SerializeField]
        public Mesh Mesh { get; set; }
        [BoxGroup("Visual")] [field: SerializeField]
        public Material Material { get; set; }
        
        [BoxGroup("Definition")] [field: SerializeField]
        public float Damages { get; set; }
        [BoxGroup("Definition")] [field: SerializeField]
        public float speed;
    }

    [Serializable]
    public struct ZoneBehaviourData : IBehaviourData
    {
        [BoxGroup("General stats")][Tooltip("Type -1 to disable Ticker.")] [field: SerializeField]
        public float TickRate { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [BoxGroup("General stats")] [field: SerializeField]
        public ScriptReference Behaviour { get; set; }
        
        [BoxGroup("Instantiation")] [field: SerializeField]
        public int Quantity { get; set; }
        [BoxGroup("General stats")] [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [BoxGroup("General stats")] [field:SerializeReference]
        public HitShape CollisionShape { get; set; }
        
        [BoxGroup("Visual")] [field: SerializeField]
        public Mesh Mesh { get; set; }
        [BoxGroup("Visual")] [field: SerializeField]
        public Material Material { get; set; }
        [BoxGroup("Definition")] [field: SerializeField]
        public float Damages { get; set; }
        [BoxGroup("Definition")] [field:SerializeReference]
        public HitShape damageZone;
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
