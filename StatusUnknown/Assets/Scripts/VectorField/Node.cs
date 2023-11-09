
namespace VectorField
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [Serializable]
    public class Node
    {
        [field: SerializeField]
        public Vector3 Position { get; private set; }
        [SerializeField]
        public float DistanceFromTarget;
        [SerializeField]
        public List<Vector3> linkedBoundPositions = new List<Vector3>();
        public List<float> linkedNodeDistance = new List<float>();

        [SerializeField]
        public Vector3 targetDirection;

        public Node(Vector3 position)
        {
            Position = position;
            DistanceFromTarget = -1;
            linkedBoundPositions = new List<Vector3>();
        }
        public void AddLink(Vector3 boundPosition, float Distance)
        {
            linkedBoundPositions.Add(boundPosition);
            linkedNodeDistance.Add(Distance);
        }

        public void RemoveLink(Vector3 boundPosition)
        {
            var index = linkedBoundPositions.IndexOf(boundPosition);
            if(index >= 0)
            {
                linkedBoundPositions.RemoveAt(index);
                linkedNodeDistance.RemoveAt(index);
            }

        }
    }
    [Serializable]
    public struct LinkedNode
    {
        public Vector3 position;
        public float distance;
    }
}