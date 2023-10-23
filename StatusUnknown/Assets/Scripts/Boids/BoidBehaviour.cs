using System.Collections.Generic;
using UnityEngine;

public abstract class BoidBehaviour : ScriptableObject
{
    public abstract Vector3 GetSteeringVector(List<Boids> surrondingBoids);
}
