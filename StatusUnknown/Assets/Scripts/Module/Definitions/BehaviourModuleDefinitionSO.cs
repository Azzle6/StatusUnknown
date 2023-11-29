namespace Module.Definitions
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;

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
        public IInstantiationRule InstantiationRule { get; set; }
    }
    
    [Serializable]
    public struct ProjectileBehaviourData : IBehaviourData
    {
        [BoxGroup("Instantiation")]
        public int quantity;
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public MonoScript Behaviour { get; set; }
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [BoxGroup("Visual")]
        public Mesh mesh;
        [BoxGroup("Visual")]
        public Material material;
        
        public LayerMask layerMask;
        
        [SerializeReference] [BoxGroup("Definition")]
        public HitShape shape;
        [BoxGroup("Definition")]
        public float speed;
        [BoxGroup("Definition")]
        public float damages;
    }

    [Serializable]
    public struct ZoneBehaviourData : IBehaviourData
    {
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public MonoScript Behaviour { get; set; }
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
        
        [SerializeReference]
        public HitShape Shape;
    }

    [Serializable]
    public struct DropBehaviourData : IBehaviourData
    {
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public MonoScript Behaviour { get; set; }
        [BoxGroup("Instantiation")]
        [field: SerializeReference]
        public IInstantiationRule InstantiationRule { get; set; }
    }

    public interface IInstantiationRule
    {
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion curRotation, int quantity);
    }

    [Serializable]
    public struct ForwardInstantiationRule : IInstantiationRule
    {
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion curRotation, int quantity)
        {
            ElementPositionInfo[] result = new ElementPositionInfo[quantity];
            for (int i = 0; i < quantity; i++)
                result[i] = new ElementPositionInfo(pos, curRotation);
            
            return result;
        }
    }

    [Serializable]
    public struct ConicInstantiationRule : IInstantiationRule
    {
        public float angle;
        public bool regular;
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion curRotation, int quantity)
        {
            ElementPositionInfo[] result = new ElementPositionInfo[quantity];
            for (int i = 0; i < quantity; i++)
            {
                Quaternion newRotation = Quaternion.AngleAxis(this.angle, Vector3.up) * curRotation;
                Debug.DrawRay(pos, newRotation * Vector3.forward, Color.blue, 100f);
                Debug.Log($"Calculated dir : {newRotation}.");
                result[i] = new ElementPositionInfo(pos, newRotation);
            }
            
            return result;
        }
    }

    /*[Serializable]
    public struct HorizontalInstantiationRule : IInstantiationRule
    {
        public float Length;
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion forward, int quantity)
        {

        }
    }*/

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
