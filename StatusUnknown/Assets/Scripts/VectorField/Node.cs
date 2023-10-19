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
    public Node(Vector3 position)
    {
        this.Position = position;
        DistanceFromTarget = -1;
        linkedBoundPositions = new List<Vector3> ();
    }
}
