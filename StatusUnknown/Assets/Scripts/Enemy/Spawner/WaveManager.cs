using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    SpawnCommand[] spawnCommands;
    int spawnFinishedCounter;
    public event Action<WaveManager> EndWaveEvent;
    public void StartWave()
    {
        spawnFinishedCounter = spawnCommands.Length;
        for (int i = 0; i < spawnCommands.Length; i++)
        {
            spawnCommands[i].SpawnCommandEndEvent += DecrementFinishedCounter;
        }
    }

    void DecrementFinishedCounter(SpawnCommand spawnCommand)
    {
        spawnCommand.SpawnCommandEndEvent -= DecrementFinishedCounter;
        spawnFinishedCounter--;
    }
    

}
