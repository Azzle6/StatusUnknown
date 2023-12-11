using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public SpawnerLister spawnerLister;
    public enum TriggerType
    {
        Stop, Start
    }

    public TriggerType triggerType;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && triggerType == TriggerType.Start)
        {
            for (int i = 0; i < spawnerLister.spawnerList.Count; i++)
            {
                spawnerLister.spawnerList[i].isSpawning = true;
            }
        }
        if (other.name == "Player" && triggerType == TriggerType.Stop)
        {
            for (int i = 0; i < spawnerLister.spawnerList.Count; i++)
            {
                spawnerLister.spawnerList[i].isSpawning = false;
            }
        }
    }
}
