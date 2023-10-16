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
            nodeField.Add(PositionToBoundPosition(node.position), node);
        return nodeField;
    }
    public static List<Node> GetLinkNode(Node node, Dictionary<Vector3,Node> nodeField)
    {
        List<Node> linkedNodes = new List<Node>();
        foreach(var dir in linkNodeDirections)
        {
            Vector3 nodeBoundPos = PositionToBoundPosition(node.position) + dir * fieldDensity;
            float depth = 0;
            while(!nodeField.ContainsKey(nodeBoundPos) && depth <= linkNodeDepth)
            {
                nodeBoundPos += Vector3.down * fieldDensity;
                depth += fieldDensity;
            }
            // TODO : Restrain link rules
            if(nodeField.ContainsKey(nodeBoundPos))
                linkedNodes.Add(nodeField[nodeBoundPos]);
        }
        return linkedNodes;
    }
    public static Node WorlPositiondToNode(Vector3 position, Dictionary<Vector3, Node> nodeField)
    {
        Node node = null;
        Vector3 boundPosition = PositionToBoundPosition(position);
        if(nodeField.ContainsKey(boundPosition))
            node = nodeField[boundPosition];

        return node;
    }

}
