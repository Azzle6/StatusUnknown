using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy.Spawner
{
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
            if(this.spwanOnEwake)
                this.StartSpawnProcess();
        }

        public void StartSpawnProcess()
        {
            this.reapeatCount = this.repeat;
            for (int i = 0; i < this.repeat; i++)
                this.StartCoroutine(this.SpawnCoroutine((i + 1) * this.delay));
        }
        IEnumerator SpawnCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            for(int i = 0; i < this.spawnQty; i++)
            {
                GameObject obj = Instantiate(this.objectToSpawn, this.spawnLocation.SpawnPosition(), Quaternion.identity);
                EnemyContext enemy = obj.GetComponent<EnemyContext>();
                if(enemy != null)
                {
                    enemy.OnDeathEvent += this.RemoveEnemy;
                    if(this.enemyContexts == null) this.enemyContexts = new HashSet<EnemyContext>();
                    this.enemyContexts.Add(enemy);
                }    
            }
            
            // End Spawn
            this.reapeatCount--;
            if(this.reapeatCount <= 0)
                this.CallEndSpawnEvent();
        }
        void RemoveEnemy(EnemyContext enemy)
        {
            enemy.OnDeathEvent -= this.RemoveEnemy;
            this.enemyContexts.Remove(enemy);
            if(this.enemyContexts.Count == 0 && this.spawnFinished)
                this.CallDeathEndEvent();

        }
        void CallDeathEndEvent()
        {
            this.DeathEndUEvent?.Invoke();
        }
        void CallEndSpawnEvent()
        {
            if (!this.spawnFinished)
            {
                this.SpawnCommandEndEvent?.Invoke(this);
                this.SpawnEndUEvent?.Invoke();
            }
            
            this.StopAllCoroutines();
            this.spawnFinished = true;
        }

        private void OnDrawGizmos()
        {

            foreach (var enemy in this.enemyContexts)
            {
                Gizmos.DrawLine(this.transform.position, enemy.transform.position);
            }
        
        }

    }
}
