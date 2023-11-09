using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour
{
    [SerializeField] MassBody massBody;

    [Header("Avoidance")]
    [SerializeField] LayerMask avoidMask;
    [SerializeField] float avoidDistance = 1;
    [SerializeField] float avoidStrength = 1;

    private void Update()
    {
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
