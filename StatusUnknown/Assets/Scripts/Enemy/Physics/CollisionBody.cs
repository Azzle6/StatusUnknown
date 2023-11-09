using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CollisionBody : MonoBehaviour
{
    [SerializeField] float gravity = 9;
    [SerializeField] SphereCollider collider;
    [SerializeField] MassBody massBody;
    float size => collider.radius;
    [SerializeField] LayerMask collisionMask;

    [Header("Hover")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float maxSlopeAngle = 20;
    [SerializeField] float slopeAngle;

    [SerializeField] float hoverHeight = 2;
    [SerializeField] float hoverStrength = 7;

    [Header("Avoidance")]
    [SerializeField] LayerMask avoidMask;
    [SerializeField] float avoidDistance = 1;
    [SerializeField] float avoidStrength = 1;

    private void FixedUpdate()
    {
        // Gravity
        

        // Ground
        RaycastHit hitGround;
        if(Physics.SphereCast(transform.position, size, Vector3.down,out hitGround, hoverHeight, groundMask))
        {
            float hitAngle = Vector3.Angle(Vector3.up,hitGround.normal);
            slopeAngle = hitAngle;
            Debug.DrawLine(transform.position, hitGround.point);
            Debug.DrawRay(hitGround.point, hitGround.normal, (hitAngle <= maxSlopeAngle) ? Color.green : Color.red);
            if(hitAngle <= maxSlopeAngle) { massBody.AddForce(Vector3.up * hoverStrength / hitGround.distance); }
        }
        else
        {
            massBody.AddForce(Vector3.down * gravity * massBody.Mass);
        }

        // Avoidance
        Vector3 avoidForce = Vector3.zero;
        var avoidColliders = Physics.OverlapSphere(transform.position, avoidDistance, avoidMask);
        foreach(var collider in avoidColliders)
        {
            Vector3 repulsePoint = collider.ClosestPoint(transform.position);
            Vector3 repulseVector = transform.position - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;

            RaycastHit hit;
            if(Physics.Raycast(transform.position,-repulseVector, out hit, avoidDistance, avoidMask))
            {
                Debug.DrawLine(transform.position, repulsePoint);
                float hitAngle = Vector3.Angle(Vector3.up, hit.normal); 
                if (hit.collider == collider && hitAngle > maxSlopeAngle)
                    avoidForce += (repulseVector * avoidStrength / (repulseMagnitude * repulseMagnitude));
            }
            
        }
        if(avoidColliders.Length > 0)
            massBody.AddForce(avoidForce/ avoidColliders.Length);

        // Solid collider
        var solidCollider = Physics.OverlapSphere(transform.position, size * 2, collisionMask);
        foreach (var collider in solidCollider)
        {
            Vector3 repulsePoint = collider.ClosestPoint(transform.position);
            Vector3 repulseVector = transform.position - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;

            if (repulseMagnitude == 0) repulseMagnitude = size;
            if (repulseMagnitude <= size) // avoid inner collision
                transform.position = repulsePoint + (repulseVector / repulseMagnitude) * size;

        }


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.down * (hoverHeight/2+size), new Vector3(size,hoverHeight,size));
        Gizmos.DrawWireSphere(transform.position, avoidDistance);
    }
}
