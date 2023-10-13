using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "VectorFieldData", menuName = "CustomAssets/VectorFieldData", order = 1)]
public class VectorFieldVolumeData : ScriptableObject
{
    [SerializeField]
    public List<Node> Nodes;
    [SerializeField]
    public List<Vector3> NodeBoundPosition;

    public void AddNode(Node node)
    {
        if(Nodes == null) { Nodes = new List<Node>(); }
        if(NodeBoundPosition == null) {  NodeBoundPosition = new List<Vector3>();}
        Nodes.Add(node);
        NodeBoundPosition.Add(VectorFieldNavigator.PositionToBoundPosition(node.position));
    }

    public void RemoveNode(Node node)
    {
        int index = Nodes.IndexOf(node);
        if(index != -1)
        {
            Nodes.RemoveAt(index);
            NodeBoundPosition.RemoveAt(index);
        }
    }

    public void ClearData()
    {
        Nodes.Clear();
        NodeBoundPosition.Clear();
    }

    public Node GetNodeFromBoundPosition(Vector3 boundPosition)
    {
        Node node = null;
        int index = NodeBoundPosition.IndexOf(boundPosition);
        if(index >= 0 && index < Nodes.Count)
            node = Nodes[index];
        return node;
    }
    public void SaveAsset()
    {
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}





