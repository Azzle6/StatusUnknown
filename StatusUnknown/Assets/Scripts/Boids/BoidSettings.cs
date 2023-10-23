using System;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomAssets/BoidSettings", fileName = "BoidSettings", order = 0)]
public class BoidSettings : ScriptableObject
{
    public float  minSpeed = 2, maxSpeed = 5;
    public float PerceptionRadius = 3;
    public BoidBehaviourContainer[] SteeringBehaviours = new BoidBehaviourContainer[0];
}
[Serializable]
public struct BoidBehaviourContainer
{
    public BoidBehaviour Behaviour;
    public float Wheight;
}