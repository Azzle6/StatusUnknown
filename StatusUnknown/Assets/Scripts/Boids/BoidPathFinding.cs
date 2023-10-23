using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomAssets/BoidBehaviours/PathFinding", fileName = "BoidPathFinding", order = 0)]
public class BoidPathFinding : BoidBehaviour
{
    public override Vector3 GetSteeringVector(List<Boids> surrondingBoids)
    {
        return Vector3.zero;
    }
}
