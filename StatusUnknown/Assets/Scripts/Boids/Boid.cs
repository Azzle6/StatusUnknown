using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
   [field:SerializeField]
   public BoidSettings settings { get; private set; }
    
    HashSet<(Vector3,float)> vectorToProcess = new HashSet<(Vector3, float)>();
    [HideInInspector] public Vector3 forward = Vector3.zero;
    Vector3 acceleration  = Vector3.zero;
    Vector3 velocity = Vector3.zero;


    private void OnEnable()
    {
        BoidRegister.RegisterBoid(this);
    }

    private void OnDisable()
    {
        BoidRegister.UnregisterBoid(this);
    }

    private void Update()
    {
        var neighbors = BoidRegister.GetNeighbors(this);
        vectorToProcess.Add((SteerTowards(BoidRegister.CohesionVector(this, neighbors)), settings.cohesionStrength));
        vectorToProcess.Add((SteerTowards(BoidRegister.AlignementVector(this, neighbors)),settings.alignementStrength));
        vectorToProcess.Add((SteerTowards(BoidRegister.AvoidanceVector(this, neighbors, settings.avoidDistance)),settings.avoidStrength));

        acceleration = Processvector(ref vectorToProcess);
        velocity += acceleration * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float speed = velocity.magnitude;
        forward =  (speed != 0)?velocity / speed:transform.forward;
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        transform.position += forward*speed * Time.deltaTime;

        transform.forward = forward;
    }
    private Vector3 Processvector(ref HashSet<(Vector3, float)> vectorToProcess)
    {
        Vector3 velocity = Vector3.zero;
        if (vectorToProcess.Count > 0)
        {
            foreach (var v in vectorToProcess)
                velocity += v.Item1 * v.Item2;
            //velocity = (velocity / vectorToProcess.Count).normalized;
            vectorToProcess.Clear();
        }
        return velocity;
    }
    Vector3 SteerTowards(Vector3 v)
    {
        v = v.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxForce);
    }

}
