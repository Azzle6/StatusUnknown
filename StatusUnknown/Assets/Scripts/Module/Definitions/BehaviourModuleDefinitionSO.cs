namespace Module.Definitions
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;
    using Random = UnityEngine.Random;

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
            
            Vector3 relativeForward = curRotation * Vector3.forward;
            
            for (int i = 0; i < quantity; i++)
                result[i] = new ElementPositionInfo(pos, Quaternion.LookRotation(relativeForward));
            
            return result;
        }
    }

    [Serializable]
    public struct ConicInstantiationRule : IInstantiationRule
    {
        [Range(0,180)]
        public float angle;
        public bool regular;
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion curRotation, int quantity)
        {
            ElementPositionInfo[] result = new ElementPositionInfo[quantity];
            for (int i = 0; i < quantity; i++)
            {
                float curAngle;
                if (this.regular)
                {
                    float displacementAngle = this.angle / quantity;
                    curAngle = displacementAngle * i - this.angle/2 + displacementAngle / 2;
                }
                else
                {
                    curAngle = Random.Range(-this.angle, this.angle);
                }
                Vector3 relativeForward = curRotation * Vector3.forward;
                Vector3 relativeUp = curRotation * Vector3.up;

                Quaternion baseRotation = Quaternion.AngleAxis(curAngle, relativeUp);
                Quaternion finalRotation = Quaternion.LookRotation(baseRotation * relativeForward);
                result[i] = new ElementPositionInfo(pos, finalRotation);
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
