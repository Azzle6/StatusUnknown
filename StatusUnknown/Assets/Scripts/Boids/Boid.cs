using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float viewDistance = 2;
    public float avoidDistance = 1;
    public float speed = 1;
    HashSet<Vector3> vectorToProcess = new HashSet<Vector3>();
    [HideInInspector]
    public Vector3 desiredDirection;
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
        vectorToProcess.Add(BoidRegister.CohesionVector(this, neighbors));
        vectorToProcess.Add(BoidRegister.AlignementVector(this, neighbors));
        vectorToProcess.Add(BoidRegister.AvoidanceVector(this, neighbors,avoidDistance));

        desiredDirection = Processvector(ref vectorToProcess);
    }

    private void FixedUpdate()
    {
        
        Quaternion rotationMultiplier = Quaternion.FromToRotation(transform.forward,desiredDirection);
        Quaternion desiredRotation = transform.rotation * rotationMultiplier;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, 90* Time.deltaTime);
        //if(desiredDirection != Vector3.zero) // TODO : SMOOTH
            //transform.forward = desiredDirection;

        transform.position += desiredDirection * speed * Time.deltaTime;
    }
    private Vector3 Processvector(ref HashSet<Vector3> vectorToProcess)
    {
        Vector3 velocity = Vector3.zero;
        if (vectorToProcess.Count > 0)
        {
            foreach (var v in vectorToProcess)
                velocity += v;
            velocity = (velocity / vectorToProcess.Count).normalized;
            vectorToProcess.Clear();
        }
        return (velocity != Vector3.zero)?velocity : transform.forward;
    }

}
