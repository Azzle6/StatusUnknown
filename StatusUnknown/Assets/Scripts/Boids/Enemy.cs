using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MassBody
{
    [Header("Body")]
    [SerializeField] SphereCollider collider;
    float size => collider.radius;

    [Header("Repulse")]
    [SerializeField] float repulseDistance = 2;
    [SerializeField] float repulseStrenght = 1;
    [SerializeField] LayerMask repulseMask;
    [SerializeField,Range(0,90)] float maxGroundAngle = 20;
    [SerializeField] float maxGroundDot;
    [SerializeField] float debugDot;

    [Header("Debug")]
    [SerializeField] 
    Gradient colorGradient = new Gradient();
    private void Update()
    {
        // Gravity
        AddForce(Vector3.down * 9 * Mass);

        // Path

        //Repulse
        var Colliders = Physics.OverlapSphere(transform.position, repulseDistance, repulseMask);
        foreach (var Collider in Colliders)
        {
            Vector3 repulsePoint = Collider.ClosestPoint(transform.position);
            Vector3 repulseVector = transform.position - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;
            if(repulseMagnitude == 0) repulseMagnitude = 1;

            if (repulseMagnitude < size) // avoid inner collision
                transform.position =repulsePoint + (repulseVector / repulseMagnitude) * size;

            RaycastHit hit;
            Physics.SphereCast(transform.position, size, -repulseVector, out hit, repulseMagnitude * 2, repulseMask);

           if (hit.collider == Collider)
           {
                // ground detection
                float groundDot = Vector3.Dot(Vector3.up, hit.normal);
                maxGroundDot = 2 * (1 -(maxGroundAngle / 90)) - 1;
                debugDot = groundDot;
                if (groundDot >= maxGroundDot)
                    AddForce(Vector3.up * repulseStrenght / repulseMagnitude);
                else
                    AddForce(repulseVector * repulseStrenght / (repulseMagnitude * repulseMagnitude));

                Debug.DrawRay(transform.position, -repulseVector, (groundDot >= maxGroundDot)?Color.green : Color.red);

           }
 
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, repulseDistance);
    }

}
