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

    [SerializeField]
    public Vector3 targetDirection;
    public Node(Vector3 position)
    {
        this.Position = position;
        DistanceFromTarget = -1;
        linkedBoundPositions = new List<Vector3> ();
    }
    public void CalcDistanceVector(ref Dictionary<Vector3,Node> nodeField)
    {
        Vector3 targetDiretcion = Vector3.zero;
        float minDistance = -1;
        foreach (var boundPosition in linkedBoundPositions)
        {
            Node node = nodeField[boundPosition];//TODO make it safe;
            if(minDistance < 0 || node.DistanceFromTarget < minDistance)
            {
                minDistance = node.DistanceFromTarget;
                targetDirection = (node.Position - Position).normalized;
            }
        }
    }
}
[Serializable]
public struct LinkedNode
{
    public Vector3 position;
    public float distance;
}
