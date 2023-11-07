using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassBody : MonoBehaviour
{
    [field: SerializeField] public float Mass {  get; private set; }
    [field: SerializeField] public float Drag { get; private set; }
    //[SerializeField] 
    Vector3 Acceleration = Vector3.zero;
    //[SerializeField] 
    Vector3 Velocity = Vector3.zero;
    
    [Button("AddForce")]
    public void AddForce( Vector3 force)
    {
        Acceleration += (force / Mass);
    }
    public void SetVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }

    private void FixedUpdate()
    {
        ProcessAcceleration();
        transform.position += Velocity * Time.deltaTime;
    }
    void ProcessAcceleration()
    {
        Acceleration -= Velocity * Drag;
        Velocity += Acceleration * Time.deltaTime;
        Acceleration = Vector3.zero;
    }
}
