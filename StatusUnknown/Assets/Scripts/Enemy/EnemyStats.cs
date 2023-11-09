using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStats", menuName = "CustomAssets/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public float health = 100;
    [Header("Aggro")]
    public float AggroRange = 10;

    [Header("Chase")]
    public float chaseStrength = 5;

    [Header("Attack")]
    public float AttackRange = 0.5f;
}
