using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class VectorFieldVolume : MonoBehaviour
{
    [SerializeField] float fieldDensity => VectorFieldNavigator.fieldDensity;
    [SerializeField] LayerMask fieldMask;
    

    // intern Data
    [SerializeField] BoxCollider fieldCollider;
    [SerializeField] VectorFieldVolumeData data;
    bool isValidData { get { return data != null; } }

    Vector3[] GetBoundsPoints(Bounds bounds) // Used in method " SetNodeField "
    {
        if (fieldDensity == 0) return new Vector3[0];
        int sizeX = Mathf.CeilToInt(bounds.size.x / fieldDensity);
        int sizeY = Mathf.CeilToInt(bounds.size.y / fieldDensity);
        int sizeZ = Mathf.CeilToInt(bounds.size.z / fieldDensity);
        int n = sizeX * sizeY * sizeZ;
        Vector3[] points = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            int x = i % sizeX;
            int z = (i / sizeX) % sizeZ;
            int y = (i / (sizeX * sizeZ)) % sizeY;
            points[i] = VectorFieldNavigator.PositionToBoundPosition(new Vector3(x, y, z) * fieldDensity + VectorFieldNavigator.PositionToBoundPosition(bounds.min));
        }
        return points;
    }
    [Button("Bake"),ShowIf("isValidData",true)]
    void RegisterNodeField()
    {
        Vector3[] boundsPoints = GetBoundsPoints(fieldCollider.bounds);
        //Debug.Log(boundsPoints.Length);
        int sizeX = Mathf.CeilToInt(fieldCollider.bounds.size.x / fieldDensity);
        int sizeZ = Mathf.CeilToInt(fieldCollider.bounds.size.z / fieldDensity);
        
        data.ClearData();
        Dictionary<Vector3, Node> nodeField = new Dictionary<Vector3, Node>();
        List<Node> nodes = new List<Node>();
        for (int i = 0; i < boundsPoints.Length; i++)
        {
            Ray ray = new Ray(boundsPoints[i], Vector3.down);
          
            RaycastHit hit;
            // Collider MUST BE CONVEX !!!
            if (Physics.Raycast(ray, out hit, fieldDensity, fieldMask) && !Physics.CheckSphere(boundsPoints[i], 0.001f, fieldMask))
                    nodes.Add(new Node(hit.point));
        }
        data.SetNodes(nodes);
        data.SaveAsset();
    }


    private void OnDrawGizmos()
    {
        if(data == null) return;
        Gizmos.color = Color.blue;
        foreach( var node in data.Nodes)
            Gizmos.DrawSphere(node.position, 0.1f);

        for(int i = 0; i < data.linkedNodes.Count; i++)
        {
            foreach (var node in data.linkedNodes[i].nodes)
                Gizmos.DrawLine(data.Nodes[i].position, node.position);
        }




    }
}
