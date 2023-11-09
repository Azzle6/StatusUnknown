using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MassBody : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;
    [field: SerializeField] public float Mass { get; private set; }
    [field: SerializeField] public float Drag { get; private set; }
    //[SerializeField] 
    Vector3 Acceleration = Vector3.zero;
    //[SerializeField] 
    Vector3 Velocity = Vector3.zero;

    [Header("Collision")]
    public bool isTrigger;
    [SerializeField] LayerMask collisionMask;


    [Button("AddForce")]
    public void AddForce(Vector3 force)
    {
        Acceleration += (force / Mass);
        //body.AddForce(force);
    }
    public void SetVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }

    private void FixedUpdate()
    {
        ProcessAcceleration();
        var nextPosition = transform.position;
        var solidCollider = Physics.OverlapSphere(nextPosition, sphereCollider.radius * 2, collisionMask);
        foreach (var collider in solidCollider)
        {
            Vector3 repulsePoint = collider.ClosestPoint(nextPosition);
            Vector3 repulseVector = nextPosition - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;

            if (repulseMagnitude == 0) repulseMagnitude = sphereCollider.radius;
            if (repulseMagnitude < sphereCollider.radius) {
                transform.position = repulsePoint + (repulseVector / repulseMagnitude) * sphereCollider.radius;
                AddForce(repulseVector);
            }
        }

        transform.position += Velocity * Time.fixedDeltaTime;
    }

    void ProcessAcceleration()
    {
        Acceleration -= Velocity * Drag;
        Velocity += Acceleration * Time.fixedDeltaTime;
        Acceleration = Vector3.zero;
    }
}
