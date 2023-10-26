using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidRegister 
{
    static HashSet<Boid> boids;
    public static void RegisterBoid(Boid boid)
    {
        if(boids == null) { boids = new HashSet<Boid>(); }
        boids.Add(boid);
    }
    public static void UnregisterBoid(Boid boid)
    { 
        boids.Remove(boid);
    }
    public static Vector3 CohesionVector(Boid boid, HashSet<Boid> neighbor)
    {
        Vector3 result = Vector3.zero;
        if(neighbor.Count <=0) return result;

        foreach(var b in neighbor)
            result += b.transform.position;
        
        result = result/neighbor.Count;
            Debug.DrawLine(result, boid.transform.position,Color.yellow);

        result = Vector3.ClampMagnitude(result - boid.transform.position,1);

        return result;
    }
    public static Vector3 AlignementVector(Boid boid, HashSet<Boid> neighbor)
    {
        Vector3 result = Vector3.zero;
        if(neighbor.Count <=0) return result;

        foreach (var b in neighbor)
            result += b.forward;
        result = Vector3.ClampMagnitude(result/neighbor.Count,1);
        Debug.DrawRay(boid.transform.position, result, Color.green);

        return result;
    }
    public static Vector3 AvoidanceVector(Boid boid, HashSet<Boid> neighbor, float avoidanceRadius)
    {
        if (neighbor.Count <= 0) return Vector3.zero;

        Vector3 result = Vector3.zero;
        int resultQty = 0;
        foreach (var b in neighbor)
        {
            Vector3 avoidVec = b.transform.position - boid.transform.position;
            if (Vector3.SqrMagnitude(avoidVec) <= avoidanceRadius * avoidanceRadius)
            {
                result -= avoidVec;
                resultQty++;
            }
        }
        if(resultQty < 0) return Vector3.zero;

        result = Vector3.ClampMagnitude((result / resultQty),1);
        Debug.DrawRay(boid.transform.position, result, Color.red);
        return result;
    }
    public static HashSet<Boid> GetNeighbors(Boid boid)
    { 
        if(boids == null) return null;

        HashSet<Boid> result = boids;
        foreach(var b in boids)
        {
            if(b != boid && Vector3.SqrMagnitude(b.transform.position - boid.transform.position) <= boid.settings.viewDistance * boid.settings.viewDistance)
            {
                Debug.DrawLine(b.transform.position, boid.transform.position, Color.blue);
                result.Add(b);
            }
        }
        return result;
    }
}
