namespace VectorField
{
    using Core.Helpers;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class VectorFieldVolume : MonoBehaviour
    {
        [SerializeField] float fieldDensity => VectorFieldNavigator.fieldDensity;
        [SerializeField] LayerMask fieldMask;
        [SerializeField] Transform target;


        // intern Data
        [SerializeField] BoxCollider fieldCollider;
        [SerializeField] VectorFieldVolumeData data;

        //debug editor
        Node worldTargetNode = null;

        private void OnEnable()
        {
            if (data != null)
                VectorFieldNavigator.RegisterVectorFieldVolume(data);
        }
        private void OnDisable()
        {
            if (data != null)
                VectorFieldNavigator.UnRegisterVectorFieldVolume(data);
        }
        private void Update()
        {
            UpdateFlowField();
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

        void UpdateFlowField()
        {
            VectorFieldNavigator.SetTargetDistance(target.position, data.NodeField);
            worldTargetNode = VectorFieldNavigator.WorlPositiondToNode(target.position, data.NodeField, 2);
        }
    }
#if UNITY_EDITOR
    public partial class VectorFieldVolume
    {
        
        bool isValidData { get { return data != null; } }
        [Header("Debug")]
        [SerializeField] Gradient distanceGradient;
        [SerializeField] int maxGradientDistance;
        [SerializeField] bool showLink, showArrow;
        [Button("Bake"), ShowIf("isValidData", true)]
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
            if (data == null) return;
            Gizmos.color = Color.blue;

            var nodeField = (Application.isPlaying) ? VectorFieldNavigator.NodeField : data.NodeField;
            foreach (var node in nodeField)
            {

                // draw Node

                Gizmos.color = distanceGradient.Evaluate(node.Value.DistanceFromTarget / maxGradientDistance);
                Gizmos.DrawSphere(node.Value.Position, 0.1f);
                //Gizmos.DrawCube(node.Value.Position + Vector3.up * node.Value.DistanceFromTarget*0.05f, new Vector3(0.1f,0.4f + node.Value.DistanceFromTarget * 0.1f,0.1f));

                // draw link
                if (showLink)
                {
                    foreach (var boundPosition in node.Value.linkedBoundPositions)
                        Gizmos.DrawLine(nodeField[boundPosition].Position, node.Value.Position);

                    
                }

                //foreach (var position in node.Value.linkPosition)
                    //Gizmos.DrawLine(node.Value.Position, position);

                if (showArrow && node.Value.targetDirection != Vector3.zero) // Draw arrow
                    DrawArrow.ForGizmo(node.Value.Position + Vector3.up * 0.1f, node.Value.targetDirection, 0.35f, 40);
            }

            if (target == null) return;
            
            if(Application.isEditor)
                UpdateFlowField();

            if (worldTargetNode != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(target.position, worldTargetNode.Position);
                Gizmos.DrawCube(worldTargetNode.Position, Vector3.one * 0.2f);
            }


        }
    }
#endif
}

