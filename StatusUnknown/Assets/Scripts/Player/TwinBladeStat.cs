using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "TwinBlade", menuName = "CustomAssets/WeaponStat/TwinBlade", order = 1)]
public class TwinBladeStat : WeaponStat
{
    [Tooltip("The total of CastTime, BuildUpTime, ActiveTime, RecoveryTime, must match the animation length")]
    public MeleeAttack[] attacks;
    public float attack3DamageDot;
    public float dotDuration;
}
