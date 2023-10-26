using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CustomAssets/BoidSettings", fileName = "BoidSettings", order = 0)]
public class BoidSettings : ScriptableObject
{
    // settings
    [Header("Move")]
    public float viewDistance = 2;
    public float avoidDistance = 1;
    public float minSpeed = 1, maxSpeed = 5;
    public float hoverDistance = 0.5f;

    [Header("Vector")]
    public float maxForce = 8;
    public float avoidStrength = 2;
    public float cohesionStrength = 1;
    public float alignementStrength = 1;
    public float pathFindingStrength = 1;

}
