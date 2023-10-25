using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFieldNavigator 
{
    // Const
    public static Vector3[] linkNodeDirections = new Vector3[] { Vector3.right, Vector3.forward, -Vector3.right, -Vector3.forward };

    // Settings
    public static float fieldDensity = 1; // TODO : Add this varaiable to inspector window in some way (WIP)
    public static float linkNodeYDist = 1.2f;

    //Data
    static HashSet<VectorFieldVolume> activeVolume; // editor scene usage;
    static HashSet<VectorFieldVolumeData> activeData; // playmode usage;

    #region Subscribe data
    public static void RegisterVolume(VectorFieldVolume volume)
    {
        Debug.Log("RegisterVolume");
        if (activeVolume == null) activeVolume = new HashSet<VectorFieldVolume>();
        activeVolume.Add(volume);

    }
    public static void UnRegisterVolume(VectorFieldVolume volume)
    {
        activeVolume.Remove(volume);
    }

    public static void Registerdata(VectorFieldVolumeData data)
    {
        if(activeData == null) activeData = new HashSet<VectorFieldVolumeData>();
        activeData.Add(data);
    }
    public static void UnRegisterdata(VectorFieldVolumeData data)
    {
        activeData.Remove(data);
    }
    #endregion

    #region Operations
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
    public static void LinkNode(Node node, Dictionary<Vector3,Node> nodeField)
    {
        foreach(var dir in linkNodeDirections)
        {
            int linkIteration = Mathf.FloorToInt(linkNodeYDist / fieldDensity);
            Vector3 originNodeBoundPos = PositionToBoundPosition(node.Position) + (dir + linkIteration * Vector3.up) * fieldDensity  ;

            for(int i = 0; i <= (linkIteration * 2); i++)
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
        targetNode.DistanceFromTarget = 0;

        HashSet<Node> checkedNode = new HashSet<Node>();
        Queue<Node> nodeToProcess = new Queue<Node>();
        nodeToProcess.Enqueue(targetNode);

            
        while(nodeToProcess.Count > 0) { 
            Node dequeueNode = nodeToProcess.Dequeue();
            foreach (var boundPosition in dequeueNode.linkedBoundPositions)
            {
                Node enqueueNode = nodeField[boundPosition];

                if (enqueueNode.DistanceFromTarget >= dequeueNode.DistanceFromTarget + 1)
                {
                    enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;
                    enqueueNode.targetDirection = (dequeueNode.Position- enqueueNode.Position)/fieldDensity;
                }
                    

                if (!checkedNode.Contains(enqueueNode)){

                    if(enqueueNode.DistanceFromTarget >= dequeueNode.DistanceFromTarget + 1)
                        enqueueNode.targetDirection = (dequeueNode.Position - enqueueNode.Position) / fieldDensity;

                    enqueueNode.DistanceFromTarget = dequeueNode.DistanceFromTarget + 1;
                    nodeToProcess.Enqueue(enqueueNode);
                    checkedNode.Add(enqueueNode);
                }
            }
            
        }

    }
    #endregion

    #region EditorUtils
    public static void BakeAllActiveVolume()
    {
        Debug.Log("Bake All");
        if (activeVolume == null) return;
        foreach (var volume in activeVolume)
            volume.RegisterNodeField();
    }
    #endregion

}
