

namespace VectorField
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using UnityEngine;

    public static class VectorFieldNavigator
    {
        public const float fieldDensity = 1f; // TODO : Add this varaiable to inspector window in some way
        public static Vector3[] linkNodeDirections = new Vector3[] 
        { 
            Vector3.right,
            new Vector3(1, 0, 1), 
            Vector3.forward, 
            new Vector3(-1, 0, 1),
            - Vector3.right, 
            new Vector3(-1, 0, -1), 
            - Vector3.forward,
            new Vector3(1, 0, -1) 
        };
        const float linkAngle = 70;
        const float linkHeight = 1f;
        public static Dictionary<Vector3, Node> NodeField;
        public static bool isActive {  get { return NodeField != null && NodeField.Count > 0; } }

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
                //int linkIteration = Mathf.FloorToInt(linkNodeYDist / fieldDensity);
                float heightLink = Mathf.Tan(linkAngle * Mathf.Deg2Rad) * fieldDensity;
                int linkIteration = Mathf.CeilToInt(heightLink / fieldDensity);
                Vector3 originNodeBoundPos = PositionToBoundPosition(node.Position) +dir * fieldDensity + Vector3.up * linkIteration * fieldDensity;

                //if (nodeField.ContainsKey(originNodeBoundPos))
                    //node.linkedBoundPositions.Add(originNodeBoundPos);

                for (int i = 0; i <= linkIteration * 2; i++)
                {
                   
                    if (nodeField.ContainsKey(originNodeBoundPos))
                    {
                        float height = Mathf.Abs(nodeField[originNodeBoundPos].Position.y - node.Position.y);
                        if (height <= heightLink && height < linkHeight)
                            node.AddLink(originNodeBoundPos,Vector3.Distance(node.Position, nodeField[originNodeBoundPos].Position));
                    }

                    originNodeBoundPos += Vector3.down * fieldDensity;
                }
            }
        }
        public static Node WorldPositiondToNode(Vector3 position, Dictionary<Vector3, Node> nodeField, float depthLinkDistance = 2)
        {
            if(NodeField == null) return null;
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
        public static Node WorldPositiondToNode(Vector3 position, float depthLinkDistance = 2)
        {
            if (NodeField == null) return null;
            Vector3 boundPosition = PositionToBoundPosition(position);
            int depthIteration = Mathf.FloorToInt(depthLinkDistance / fieldDensity);
            for (int i = 0; i < depthIteration; i++)
            {
                if (NodeField.ContainsKey(boundPosition))
                    return NodeField[boundPosition];
                boundPosition += Vector3.down * fieldDensity;
            }
            return null;
        }

        public static void SetTargetDistance(Vector3 targetPosition, Dictionary<Vector3, Node> nodeField)
        {
            Node targetNode = WorldPositiondToNode(targetPosition, nodeField,4);
            if (targetNode == null) return;
            targetNode.DistanceFromTarget = 0;

            HashSet<Node> checkedNode = new HashSet<Node>();
            Queue<Node> nodeToProcess = new Queue<Node>();
            nodeToProcess.Enqueue(targetNode);


            while (nodeToProcess.Count > 0)
            {
                Node dequeueNode = nodeToProcess.Dequeue();
                for(int i = 0; i < dequeueNode.linkedBoundPositions.Count; i++)
                {
                    Node enqueueNode = nodeField[dequeueNode.linkedBoundPositions[i]];
                    float linkDist = dequeueNode.linkedNodeDistance[i];

                    if (!checkedNode.Contains(enqueueNode))
                    {
                        enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + linkDist;
                        enqueueNode.targetDirection = (dequeueNode.Position - enqueueNode.Position) /linkDist;

                        nodeToProcess.Enqueue(enqueueNode);
                        checkedNode.Add(enqueueNode);
                    }
                    else
                    {
                        if(enqueueNode.DistanceFromTarget > dequeueNode.DistanceFromTarget + linkDist)
                        {
                            enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + linkDist;
                            enqueueNode.targetDirection = (dequeueNode.Position - enqueueNode.Position) / linkDist;
                        }
                            
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