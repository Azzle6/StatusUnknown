using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    public ProtoSpawner spawner;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            spawner.isSpawning = true;
        }
    }
}
