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
    public List<LinkedNodes> linkedNodes;
    public Dictionary<Vector3, Node> NodeField 
    { 
        get 
        { 
            if(nodeField == null)
                nodeField = VectorFieldNavigator.GenerateNodeField(Nodes);
            return nodeField;
        }
    }
    Dictionary<Vector3, Node> nodeField;
    
    public void SetNodes(List<Node> Nodes)
    {
       this.Nodes = Nodes;
        nodeField = VectorFieldNavigator.GenerateNodeField(Nodes);
        linkedNodes = new List<LinkedNodes>();
        for(int i = 0; i < Nodes.Count; i++)
            linkedNodes.Add(new LinkedNodes(VectorFieldNavigator.GetLinkNode(Nodes[i], NodeField))); 
    }

    public void ClearData()
    {
        Nodes.Clear();
        nodeField = new Dictionary<Vector3, Node>();
    }

    public void SaveAsset()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}





