using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public List<ProtoSpawner> spawnerList;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            for (int i = 0; i < spawnerList.Count; i++)
            {
                spawnerList[i].isSpawning = true;
            }
        }
    }
}
