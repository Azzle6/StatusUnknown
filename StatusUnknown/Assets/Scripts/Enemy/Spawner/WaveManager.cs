using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    SpawnCommand[] spawnCommands;
    public enum EndCondtion { TIME, COUNT, AUTO}
    [SerializeField] EndCondtion endCondtion;
}
