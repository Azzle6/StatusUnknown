using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Wave : MonoBehaviour
{
    [SerializeField] Spawn[] enemySpawner;
    [SerializeField] float warningDelay = 0.5f;
    public event Action DeathEvent;
    public bool finished {  get; private set; }
    HashSet<EnemyContext> enemyContexts = new HashSet<EnemyContext>();
    int enemyDeathCounter;
    [SerializeField] bool spawnOnAwake;
    private void Awake()
    {
        if(spawnOnAwake)
            ProcessWave();
    }
    public void ProcessWave()
    {
        foreach (Spawn spawner in enemySpawner)
            StartCoroutine(ProcessSpawn(spawner));
    }
    IEnumerator ProcessSpawn(Spawn spawn)
    {
        yield return new WaitForSeconds(spawn.delay);
        // Play warning VFX here
        yield return new WaitForSeconds(spawn.delay + warningDelay);
        GameObject obj = Instantiate(spawn.enemy, spawn.spawnPosition, Quaternion.identity);
        EnemyContext enemy = obj.GetComponent<EnemyContext>();
        if (enemy != null) AddEnemy(enemy);
    }

    void RemoveEnemy(EnemyContext enemy)
    {
        enemy.OnDeathEvent -= RemoveEnemy;
        enemyContexts.Remove(enemy);
        enemyDeathCounter++;

        if (enemyDeathCounter >= enemySpawner.Length)
        {
            Debug.Log("Death Event");
            DeathEvent?.Invoke();
        }
            
    }
    public void AddEnemy(EnemyContext enemy)
    {
        if (enemyContexts.Contains(enemy)) return;
        enemyContexts.Add(enemy);
        enemy.OnDeathEvent += RemoveEnemy;
    }
}
[System.Serializable]
public struct Spawn
{
    public GameObject enemy;
    [SerializeField]
    public Transform[] spawnPoints;
    public Vector3 spawnPosition { get { return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position; } }

       

    public float delay;
}
