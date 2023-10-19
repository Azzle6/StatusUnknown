using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFieldNavigator 
{
    public static float fieldDensity = 1; // TODO : Add this varaiable to inspector window in some way
    public static Vector3[] linkNodeDirections = new Vector3[] { Vector3.right, Vector3.forward, -Vector3.right, -Vector3.forward };
    public static float linkNodeDepth = 0.1f;
    public static Vector3 PositionToBoundPosition(Vector3 position)
    {
        int posX = Mathf.RoundToInt(position.x / fieldDensity);
        int posY = Mathf.RoundToInt(position.y / fieldDensity);
        int posZ = Mathf.RoundToInt(position.z / fieldDensity);
        return new Vector3(posX, posY, posZ) * fieldDensity;

    }
    public static Dictionary<Vector3,Node> GenerateNodeField(List<Node> nodes)
    {
        Dictionary<Vector3,Node> nodeField = new Dictionary<Vector3,Node>();
        foreach (Node node in nodes)
            nodeField.Add(PositionToBoundPosition(node.Position), node);
        return nodeField;
    }
    public static void SetLinkNode(Node node, Dictionary<Vector3,Node> nodeField)
    {

        foreach(var dir in linkNodeDirections)
        {
            Vector3 nodeBoundPos = PositionToBoundPosition(node.Position) + dir * fieldDensity;
            float depth = 0;
            while(!nodeField.ContainsKey(nodeBoundPos) && depth <= linkNodeDepth)
            {
                nodeBoundPos += Vector3.down * fieldDensity;
                depth += fieldDensity;
            }
            // TODO : Restrain link rules
            if(nodeField.ContainsKey(nodeBoundPos))
                node.linkedBoundPositions.Add(nodeBoundPos);
        }
    }
    public static Node WorlPositiondToNode(Vector3 position, Dictionary<Vector3, Node> nodeField, float depthLinkDistance = 1)
    {
        Vector3 boundPosition = PositionToBoundPosition(position);
        int depthIteration = Mathf.FloorToInt(depthLinkDistance / fieldDensity);
        for(int i = 0; i < depthIteration; i++)
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
        if(targetNode == null) return;

        HashSet<Node> checkedNode = new HashSet<Node>{targetNode};
        Queue<Node> nodeToProcess = new Queue<Node>();

        foreach (var boundPosition in targetNode.linkedBoundPositions)
        {
            Node node = nodeField[boundPosition];
            node.DistanceFromTarget = 1;
            checkedNode.Add(node);
            nodeToProcess.Enqueue(node);
        }
            
        while(nodeToProcess.Count > 0) { 
            Node dequeueNode = nodeToProcess.Dequeue();
            foreach (var boundPosition in dequeueNode.linkedBoundPositions)
            {
                Node enqueueNode = nodeField[boundPosition];

                if (enqueueNode.DistanceFromTarget > dequeueNode.DistanceFromTarget + 1)
                    enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;

                if (!checkedNode.Contains(enqueueNode)){
                    checkedNode.Add(enqueueNode);
                    enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;
                    nodeToProcess.Enqueue(enqueueNode);
                }
            }
            
        }

    }

}
