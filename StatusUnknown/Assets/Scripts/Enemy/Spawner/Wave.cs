namespace Enemy.Spawner
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Wave : MonoBehaviour
    {
        [SerializeField] Spawn[] enemySpawner;
        [SerializeField] float warningDelay = 0.5f;
        public event Action DeathEvent;
        [field : SerializeField]
        public bool finished {  get; private set; }
        HashSet<EnemyContext> enemyContexts = new HashSet<EnemyContext>();
        int enemyDeathCounter;
        [SerializeField] bool spawnOnAwake;
        [SerializeField] GameObject VFX_Spawn;
        private void Awake()
        {
            if(this.spawnOnAwake)
                this.ProcessWave();
        }
        public void ProcessWave()
        {
            Debug.Log("ProcessWave "+this.gameObject.name);
            foreach (Spawn spawner in this.enemySpawner)
                this.StartCoroutine(this.ProcessSpawn(spawner));
        }
        IEnumerator ProcessSpawn(Spawn spawn)
        {
            yield return new WaitForSeconds(spawn.delay);
            // Play warning VFX here
            if(VFX_Spawn != null)
            {
                var VFXobj = Instantiate(VFX_Spawn, spawn.spawnPosition, Quaternion.identity);
                Destroy(VFXobj, warningDelay);
            }
            yield return new WaitForSeconds(spawn.delay + this.warningDelay);
            GameObject obj = Instantiate(spawn.enemy, spawn.spawnPosition, Quaternion.identity);
            EnemyContext enemy = obj.GetComponent<EnemyContext>();
            if (enemy != null) this.AddEnemy(enemy);
        }

        void RemoveEnemy(EnemyContext enemy)
        {
            enemy.OnDeathEvent -= this.RemoveEnemy;
            this.enemyContexts.Remove(enemy);
            this.enemyDeathCounter++;

            if (this.enemyDeathCounter >= this.enemySpawner.Length)
            {
                Debug.Log("Death Event "+ this.gameObject.name);
                this.finished = true;
                this.DeathEvent?.Invoke();
            }
            
        }
        public void AddEnemy(EnemyContext enemy)
        {
            if (this.enemyContexts.Contains(enemy)) return;
            this.enemyContexts.Add(enemy);
            enemy.OnDeathEvent += this.RemoveEnemy;
        }
    }
    [System.Serializable]
    public struct Spawn
    {
        public GameObject enemy;
        [SerializeField]
        public Transform[] spawnPoints;
        public Vector3 spawnPosition { get { return this.spawnPoints[UnityEngine.Random.Range(0, this.spawnPoints.Length)].position; } }

       

        public float delay;
    }
}