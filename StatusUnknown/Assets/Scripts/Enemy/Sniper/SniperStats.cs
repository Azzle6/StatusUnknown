using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SniperStats", menuName = "CustomAssets/EnemyStats/SniperStats", order = 1)]
public class SniperStats : EnemyStats
{
    [Header("Teleportation")]
    public float minTpRange = 4f;
    public float maxTpRange = 10f;
    public float tpCooldown = 2f;
    [Header("Shoot")]
    public float shootDelay = 0.5f;
}
