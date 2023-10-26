

namespace VectorField
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using UnityEngine;

    public static class VectorFieldNavigator
    {
        public static float fieldDensity = 1f; // TODO : Add this varaiable to inspector window in some way
        public static Vector3[] linkNodeDirections = new Vector3[] { Vector3.right, Vector3.forward, -Vector3.right, -Vector3.forward };
        public static float linkNodeYDist = 1.2f;
        public static Dictionary<Vector3, Node> NodeField;


        public static Vector3 PositionToBoundPosition(Vector3 position)
        {
            int posX = Mathf.RoundToInt(position.x / fieldDensity);
            int posY = Mathf.RoundToInt(position.y / fieldDensity);
            int posZ = Mathf.RoundToInt(position.z / fieldDensity);
            return new Vector3(posX, posY, posZ) * fieldDensity;

        }
        public static Dictionary<Vector3, Node> GenerateNodeField(List<Node> nodes)
        {
            Dictionary<Vector3, Node> nodeField = new Dictionary<Vector3, Node>();
            foreach (Node node in nodes)
                nodeField.Add(PositionToBoundPosition(node.Position), node);
            return nodeField;
        }
        public static void LinkNode(Node node, Dictionary<Vector3, Node> nodeField)
        {
            foreach (var dir in linkNodeDirections)
            {
                int linkIteration = Mathf.FloorToInt(linkNodeYDist / fieldDensity);
                Vector3 originNodeBoundPos = PositionToBoundPosition(node.Position) + (dir + linkIteration * Vector3.up) * fieldDensity;

                for (int i = 0; i <= linkIteration * 2; i++)
                {
                    if (nodeField.ContainsKey(originNodeBoundPos))
                        node.linkedBoundPositions.Add(originNodeBoundPos);

                    originNodeBoundPos += Vector3.down * fieldDensity;
                }
            }
        }

        public static Node WorlPositiondToNode(Vector3 position, Dictionary<Vector3, Node> nodeField, float depthLinkDistance = 1)
        {
            Vector3 boundPosition = PositionToBoundPosition(position);
            int depthIteration = Mathf.FloorToInt(depthLinkDistance / fieldDensity);
            for (int i = 0; i < depthIteration; i++)
            {
                if (nodeField.ContainsKey(boundPosition))
                    return nodeField[boundPosition];
                boundPosition += Vector3.down * fieldDensity;
            }
            return null;
        }
        public static void SetTargetDistance(Vector3 targetPosition, Dictionary<Vector3, Node> nodeField)
        {
            Node targetNode = WorlPositiondToNode(targetPosition, nodeField);
            if (targetNode == null) return;
            targetNode.DistanceFromTarget = 0;

            HashSet<Node> checkedNode = new HashSet<Node>();
            Queue<Node> nodeToProcess = new Queue<Node>();
            nodeToProcess.Enqueue(targetNode);


            while (nodeToProcess.Count > 0)
            {
                Node dequeueNode = nodeToProcess.Dequeue();
                foreach (var boundPosition in dequeueNode.linkedBoundPositions)
                {
                    Node enqueueNode = nodeField[boundPosition];

                    if (enqueueNode.DistanceFromTarget >= dequeueNode.DistanceFromTarget + 1)
                    {
                        enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;
                        enqueueNode.targetDirection = (dequeueNode.Position - enqueueNode.Position) / fieldDensity;
                    }


                    if (!checkedNode.Contains(enqueueNode))
                    {

                        if (enqueueNode.DistanceFromTarget >= dequeueNode.DistanceFromTarget + 1)
                            enqueueNode.targetDirection = (dequeueNode.Position - enqueueNode.Position) / fieldDensity;

                        enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;
                        nodeToProcess.Enqueue(enqueueNode);
                        checkedNode.Add(enqueueNode);
                    }
                }

            }

        }

        public static void RegisterVectorFieldVolume(VectorFieldVolumeData data)
        {
            if (NodeField == null) NodeField = new Dictionary<Vector3, Node>();
            foreach (var node in data.Nodes)
                NodeField.Add(PositionToBoundPosition(node.Position), node);

        }

        public static void UnRegisterVectorFieldVolume(VectorFieldVolumeData data)
        {
            foreach (var node in data.Nodes)
                NodeField.Remove(PositionToBoundPosition(node.Position));
        }

    }
}