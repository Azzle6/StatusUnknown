using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node 
{
    [field: SerializeField]
    public Vector3 position { get; private set; }
    [HideInInspector]
    List<Node> linkedNodes = new List<Node>();
    [HideInInspector]
    List<int> nodeDistances = new List<int>();

    public Node(Vector3 position)
    {
        this.position = position;
        linkedNodes = new List<Node>();
        nodeDistances = new List<int>();
    }
}
