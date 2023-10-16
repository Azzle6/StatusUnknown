using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node 
{
    [field: SerializeField]
    public Vector3 position { get; private set; }

    public Node(Vector3 position)
    {
        this.position = position;
    }
}
