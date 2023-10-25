using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class VectorFieldVolume : MonoBehaviour
{
    [SerializeField] float fieldDensity => VectorFieldNavigator.fieldDensity;
    [SerializeField] LayerMask fieldMask;
    

    // intern Data
    [SerializeField] BoxCollider fieldCollider;
    [SerializeField] VectorFieldVolumeData data;
    bool isValidData { get { return data != null; } }
    [Header("Debug")]
    [SerializeField] Transform target;
    [SerializeField] Gradient distanceGradient;
    [SerializeField] int maxGradientDistance;
    [SerializeField] bool showLink, showArrow;

    private void OnEnable()
    {
        if (Application.isPlaying && data != null)
            VectorFieldNavigator.Registerdata(data);

        if (Application.isEditor)
            VectorFieldNavigator.RegisterVolume(this);
    }

    private void OnDisable()
    {
        if (Application.isPlaying && data != null)
            VectorFieldNavigator.UnRegisterdata(data);

        if (Application.isEditor)
            VectorFieldNavigator.UnRegisterVolume(this);
    }
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
    public void RegisterNodeField()
    {
        if(data == null) return;
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
        foreach( var node in data.NodeField)
        {
            
            // draw Node
            
            Gizmos.color = distanceGradient.Evaluate(node.Value.DistanceFromTarget / maxGradientDistance);
            Gizmos.DrawSphere(node.Value.Position, 0.1f * fieldDensity);
            //Gizmos.DrawCube(node.Value.Position + Vector3.up * node.Value.DistanceFromTarget*0.05f, new Vector3(0.1f,0.4f + node.Value.DistanceFromTarget * 0.1f,0.1f));

            // draw link
            if (showLink)
            {
                foreach (var boundPosition in node.Value.linkedBoundPositions)
                    Gizmos.DrawLine(data.NodeField[boundPosition].Position, node.Value.Position);
            }
            
            if(showArrow) // Draw arrow
                DrawArrow.ForGizmo(node.Value.Position + Vector3.up * 0.1f, node.Value.targetDirection,0.35f,40) ;
        }
            

        if (target == null) return;
        Node worldNode = VectorFieldNavigator.WorlPositiondToNode(target.position, data.NodeField,3);
        VectorFieldNavigator.SetTargetDistance(target.position, data.NodeField);
        
        if(worldNode != null)
        {
            Gizmos.color= Color.red;
            Gizmos.DrawLine(target.position, worldNode.Position);
            Gizmos.DrawCube(worldNode.Position, Vector3.one * 0.2f);
        }
            

    }
}
