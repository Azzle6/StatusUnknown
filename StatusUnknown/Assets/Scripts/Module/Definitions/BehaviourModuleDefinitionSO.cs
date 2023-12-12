namespace Module.Definitions
{
    using System;
    using Combat.HitProcess;
    using Core.Helpers;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.VFX;

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
        public float LifeTime { get; set; }
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
        [field: SerializeField]
        public ScriptReference Behaviour { get; set; }
        [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [field:SerializeReference]
        public HitShape CollisionShape { get; set; }
        
        //Visuals
        [field: SerializeField]
        public Mesh Mesh { get; set; }
        [field: SerializeField]
        public Material Material { get; set; }
        [SerializeField] 
        public VisualEffectAsset shootVFX;
        [SerializeField] 
        public VisualEffectAsset projectileVFX;
        [SerializeField] 
        public VisualEffectAsset hitVFX;
        
        //Stats
        [field: SerializeField]
        public float TickRate { get; set; }
        [field : SerializeField]
        public float LifeTime { get; set; }
        [field: SerializeField]
        public int Quantity { get; set; }
        [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        [field: SerializeField]
        public float Damages { get; set; }
        [field: SerializeField]
        public float speed;
    }

    [Serializable]
    public struct ZoneBehaviourData : IBehaviourData
    {
        [field: SerializeField]
        public ScriptReference Behaviour { get; set; }
        [field: SerializeField]
        public LayerMask LayerMask { get; set; }
        [field:SerializeReference]
        public HitShape CollisionShape { get; set; }
        
        //Visuals
        [field: SerializeField]
        public Mesh Mesh { get; set; }
        [field: SerializeField]
        public Material Material { get; set; }
        [SerializeField]
        public VisualEffectAsset zoneBurstVFX;
        [SerializeField]
        public VisualEffectAsset hitVFX;
        
        //Stats
        [field: SerializeField]
        public float TickRate { get; set; }
        [field : SerializeField]
        public float LifeTime { get; set; }
        [field: SerializeField]
        public int Quantity { get; set; }
        [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        [field: SerializeField]
        public float Damages { get; set; }
        [SerializeField]
        public HitSphere DamageRadius;
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
