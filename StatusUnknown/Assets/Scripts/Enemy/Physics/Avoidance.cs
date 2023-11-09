using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour
{
    [SerializeField] MassBody massBody;
    [SerializeField] LayerMask groundLayer;
    float groundDist = 10f;
    float checkGroundRadius = 0.5f;
    [Header("Avoidance")]
    [SerializeField] LayerMask avoidMask;
    [SerializeField] float avoidDistance = 1;
    [SerializeField] float avoidStrength = 1;
    [SerializeField] float maxGroundAngle = 40;

    private void Update()
    {
        // detect ground
        RaycastHit groundHit;
        Quaternion groundTransform = Quaternion.identity;
        if (Physics.SphereCast(transform.position, checkGroundRadius, Vector3.down, out groundHit, groundDist, groundLayer))
        {
            Plane p = new Plane(groundHit.normal, groundHit.point);
            groundTransform = Quaternion.FromToRotation(Vector3.up, groundHit.normal);
        }
        // avoidance
        Vector3 avoidForce = Vector3.zero;
        var avoidColliders = Physics.OverlapSphere(transform.position, avoidDistance, avoidMask);
        foreach (var collider in avoidColliders)
        {
            Vector3 repulsePoint = collider.ClosestPoint(transform.position);
            Vector3 repulseVector = transform.position - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -repulseVector, out hit, avoidDistance, avoidMask))
            {
                Debug.DrawLine(transform.position, repulsePoint);
                float hitAngle = Vector3.Angle(Vector3.up, hit.normal);
                avoidForce += (repulseVector * avoidStrength / (repulseMagnitude * repulseMagnitude));
            }

        }
        if (avoidColliders.Length > 0)
            massBody.AddForce(avoidForce / avoidColliders.Length);
    }
}
