using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Player;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoSpawner : MonoBehaviour
{
    public Collider trigger;
    public List<Transform> spawnPoints;
    public List<GameObject> wave;
    public float Timer;
    public bool onCD;
    public bool isSpawning;
    
    void Start()
    {
        isSpawning = false;
        onCD = false;
    }

    void Update()
    {
        if (isSpawning == false && onCD == false)
        {
            Debug.Log("Start Spawning");
            SpawnWave();
            StartCoroutine("Cooldown");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        isSpawning = true;
    }

    private void SpawnWave()
    {
        for (int i = 0; i < wave.Count; i++)
        {
            Instantiate(wave[i], spawnPoints[i]);
            Debug.Log(i);
        }
    }
    
    IEnumerator Cooldown()
    {
        onCD = true;
        yield return new WaitForSeconds(Timer);
        onCD = false;
    }
}
