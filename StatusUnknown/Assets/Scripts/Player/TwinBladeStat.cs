using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[CreateAssetMenu(fileName = "TwinBlade", menuName = "CustomAssets/WeaponStat/TwinBlade", order = 1)]
public class TwinBladeStat : WeaponStat
{
    public MeleeAttack[] attacks;
    public float attack3DamageDot;
    public float dotDuration;
}
