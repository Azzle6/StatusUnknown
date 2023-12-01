using Sirenix.OdinInspector.Editor.Internal.UIToolkitIntegration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpawnTest : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField, SerializeReference] SpawnLocation spawnLocation = new SimpleSpawnLocation();
    [SerializeField] int spawnQty =1;
    [SerializeField] float delay = 0;
    [SerializeField] int repeat = 1;

    //Spawn event
    public event Action<SpawnTest> SpawnCommandEndEvent;
    float reapeatCount;
    bool spawnFinished = false;
    public UnityEvent SpawnEndUEvent;
    public UnityEvent DeathEndUEvent;
    // Track Enemies
    public HashSet<EnemyContext> enemyContexts = new HashSet<EnemyContext>();

    [Header("Debug")]
    [SerializeField] bool spwanOnEwake;

    private void Awake()
    {
        if(spwanOnEwake)
            StartSpawnProcess();
    }

    public void StartSpawnProcess()
    {
        reapeatCount = repeat;
        for (int i = 0; i < repeat; i++)
            StartCoroutine(SpawnCoroutine((i + 1) * delay));
    }
    IEnumerator SpawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < spawnQty; i++)
        {
            GameObject obj = Instantiate(objectToSpawn, spawnLocation.SpawnPosition(), Quaternion.identity);
            EnemyContext enemy = obj.GetComponent<EnemyContext>();
            if(enemy != null)
            {
                enemy.OnDeathEvent += RemoveEnemy;
                if(enemyContexts == null) enemyContexts = new HashSet<EnemyContext>();
                enemyContexts.Add(enemy);
            }    
        }
            
        // End Spawn
        reapeatCount--;
        if(reapeatCount <= 0)
            CallEndSpawnEvent();
    }
    void RemoveEnemy(EnemyContext enemy)
    {
        enemy.OnDeathEvent -= RemoveEnemy;
        enemyContexts.Remove(enemy);
        if(enemyContexts.Count == 0 && spawnFinished)
            CallDeathEndEvent();

    }
    void CallDeathEndEvent()
    {
        DeathEndUEvent?.Invoke();
    }
    void CallEndSpawnEvent()
    {
        if (!spawnFinished)
        {
            SpawnCommandEndEvent?.Invoke(this);
            SpawnEndUEvent?.Invoke();
        }
            
        StopAllCoroutines();
        spawnFinished = true;
    }

    private void OnDrawGizmos()
    {

            foreach (var enemy in enemyContexts)
            {
                Gizmos.DrawLine(transform.position, enemy.transform.position);
            }
        
    }

}
