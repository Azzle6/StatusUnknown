using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SniperStats", menuName = "CustomAssets/EnemyStats/SniperStats", order = 1)]
public class SniperStats : EnemyStats
{
    [Header("Teleportation")]
    public float minTpRange = 4f;
    public float maxTpRange = 10f;
    [SerializeField]
    float tpCooldown = 2f;
    [SerializeField, Range(0f, 1f)]
    float tpCooldownRnd = 0.5f;
    public float TpRndCooldown { get {  return tpCooldown * (1 - Random.Range(0, tpCooldownRnd)); }  }
    [Header("Shoot")]
    public float shootDelay = 0.5f;

}
