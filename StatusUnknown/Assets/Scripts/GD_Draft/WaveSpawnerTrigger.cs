using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerTrigger : MonoBehaviour
{
    public List<GameObject> doorBlockers;
    private bool active = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player" && active)
        {
            active = false;
            LockDoors();
            GetComponent<WaveManager>().StartWaveProcess();
        }
    }

    public void LockDoors()
    {
            for (int i = 0; i < doorBlockers.Count; i++)
            {
                doorBlockers[i].SetActive(true);
            }
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < doorBlockers.Count; i++)
        {
            doorBlockers[i].SetActive(false);
        }
    }
}
