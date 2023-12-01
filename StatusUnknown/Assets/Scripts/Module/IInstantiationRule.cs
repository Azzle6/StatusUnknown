namespace Module
{
    using System;
    using Definitions;
    using UnityEngine;

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
        [Tooltip("Change the range of instantiation position. Use it to make it spawn further or at random position in the radius.")]
        public float radius;
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
                    curAngle = UnityEngine.Random.Range(-this.angle, this.angle);
                }
                Vector3 relativeForward = curRotation * Vector3.forward;
                Vector3 relativeUp = curRotation * Vector3.up;

                Quaternion baseRotation = Quaternion.AngleAxis(curAngle, relativeUp);
                Quaternion finalRotation = Quaternion.LookRotation(baseRotation * relativeForward);

                Vector3 finalPosition = pos + finalRotation * Vector3.forward * (this.regular ? this.radius : UnityEngine.Random.Range(0, this.radius));
                
                result[i] = new ElementPositionInfo(finalPosition, finalRotation);
            }
            
            return result;
        }
    }
    
    [Serializable]
    public struct HorizontalInstantiationRule : IInstantiationRule
    {
        public float Length;
        public ElementPositionInfo[] GetInstantiationInfo(Vector3 pos, Quaternion curRotation, int quantity)
        {
            ElementPositionInfo[] result = new ElementPositionInfo[quantity];

            for (int i = 0; i < quantity; i++)
            {
                Vector3 relativeForward = curRotation * Vector3.forward;
                Vector3 relativeUp = curRotation * Vector3.up;

                Quaternion horizontalRotation = Quaternion.AngleAxis(90f, relativeUp);
                Vector3 horizontalDirection = (horizontalRotation * relativeForward).normalized;

                float decalDistance = this.Length / (quantity - 1);
                Vector3 startPos = pos + horizontalDirection * (-this.Length / 2);
                Vector3 posResult = startPos + horizontalDirection * (decalDistance * i);

                result[i] = new ElementPositionInfo(posResult, curRotation);
            }
            
            
            return result;
        }
    }
}
